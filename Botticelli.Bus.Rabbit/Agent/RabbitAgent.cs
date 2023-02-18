using System.Text.Json;
using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Bus.Rabbit.Settings;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Botticelli.Bus.Rabbit.Agent;

/// <summary>
///     RabbitMQ agent
/// </summary>
/// <typeparam name="TBot" />
/// <typeparam name="THandler"></typeparam>
public class RabbitAgent<TBot, THandler> : IBotticelliBusAgent<THandler>
        where TBot : IBot
        where THandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    private readonly TBot _bot;
    private readonly IList<THandler> _handlers = new List<THandler>(5);
    private readonly ILogger<RabbitAgent<TBot, THandler>> _logger;
    private readonly IConnectionFactory _rabbitConnectionFactory;
    private readonly RabbitBusSettings _settings;

    public RabbitAgent(TBot bot, IConnectionFactory rabbitConnectionFactory, RabbitBusSettings settings)
    {
        _bot = bot;
        _rabbitConnectionFactory = rabbitConnectionFactory;
        _settings = settings;
    }

    /// <summary>
    ///     Sends a response
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <param name="timeoutMs"></param>
    /// <returns></returns>
    public async Task SendResponse(SendMessageRequest request,
                                   CancellationToken token,
                                   int timeoutMs = 10000)
    {
        try
        {
            _logger.LogDebug($"{nameof(SendResponse)}({request.Uid}) start...");
            var policy = Policy.Handle<RabbitMQClientException>()
                               .WaitAndRetryAsync(5, n => TimeSpan.FromSeconds(3 * Math.Exp(n)));

            await policy.ExecuteAsync(() => InnerSend(request));

            _logger.LogDebug($"{nameof(SendResponse)}({request.Uid}) finished");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending a response: {ex.Message}");
        }
    }

    /// <summary>
    ///     Subscribes with a new handler
    /// </summary>
    /// <param name="handler"></param>
    public async Task Subscribe(THandler handler, CancellationToken token)
    {
        _logger.LogDebug($"{nameof(Subscribe)}({typeof(THandler).Name}) start...");
        _handlers.Add(handler);
        using var connection = _rabbitConnectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        var queue = GetRequestQueueName();
        var queueDeclareResult = channel.QueueDeclare(queue);

        _logger.LogDebug($"{nameof(Subscribe)}({typeof(THandler).Name}) queue declare: {queueDeclareResult.QueueName}");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            _logger.LogDebug($"{nameof(Subscribe)}() message received");

            var deserialized = JsonSerializer.Deserialize<SendMessageRequest>(ea.Body.ToArray());
            var policy = Policy.HandleResult<MessageSentStatus>(result => result != MessageSentStatus.OK)
                               .WaitAndRetry(3, _ => TimeSpan.FromSeconds(1));

            policy.Execute(() => handler.Handle(deserialized).Result.MessageSentStatus);
        };

        channel.BasicConsume(queue,
                             true,
                             consumer);
    }

    private async Task InnerSend(SendMessageRequest request)
    {
        using var connection = _rabbitConnectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        var rk = GetRkName();
        var queue = GetResponseQueueName();

        channel.QueueDeclare(queue);
        channel.QueueBind(queue, _settings.Exchange, rk);
        channel.BasicPublish(_settings.Exchange, rk, body: JsonSerializer.SerializeToUtf8Bytes(request));
    }

    private static string GetResponseQueueName() => $"{nameof(SendMessageRequest)}_{typeof(TBot).Name}_response";
    private static string GetRequestQueueName() => $"{nameof(SendMessageRequest)}_{typeof(TBot).Name}_request";
    private static string GetRkName() => $"{nameof(SendMessageRequest)}_{typeof(TBot).Name}_response_rk";
}