using System.Text.Json;
using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Bus.Rabbit.Settings;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.DependencyInjection;
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
public class RabbitAgent<TBot, THandler> : BasicFunctions<TBot>, IBotticelliBusAgent<THandler>
        where TBot : IBot
        where THandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    private readonly IList<THandler> _handlers = new List<THandler>(5);
    private readonly ILogger<RabbitAgent<TBot, THandler>> _logger;
    private readonly IConnectionFactory _rabbitConnectionFactory;
    private readonly RabbitBusSettings _settings;
    private readonly IServiceProvider _sp;
    private bool _isActive = false;

    public RabbitAgent(IConnectionFactory rabbitConnectionFactory,
                       IServiceProvider sp,
                       RabbitBusSettings settings,
                       ILogger<RabbitAgent<TBot, THandler>> logger)
    {
        _rabbitConnectionFactory = rabbitConnectionFactory;
        _sp = sp;
        _settings = settings;
        _logger = logger;
    }

    /// <summary>
    ///     Returns response to a bus
    /// </summary>
    /// <param name="response"></param>
    /// <param name="token"></param>
    /// <param name="timeoutMs"></param>
    /// <returns></returns>
    public async Task SendResponse(SendMessageResponse response,
                                   CancellationToken token,
                                   int timeoutMs = 10000)
    {
        try
        {
            _logger.LogDebug($"{nameof(SendResponse)}({response.Uid}) start...");

            var policy = Policy.Handle<RabbitMQClientException>()
                               .WaitAndRetryAsync(5, n => TimeSpan.FromSeconds(3 * Math.Exp(n)));

            await policy.ExecuteAsync(() => InnerSend(response));

            _logger.LogDebug($"{nameof(SendResponse)}({response.Uid}) finished");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending a response: {ex.Message}");
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _isActive = true;
        await Subscribe(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _isActive = false;
        Thread.Sleep(3000);
    }

    /// <summary>
    ///     Subscribes with a new handler
    /// </summary>
    public async Task Subscribe(CancellationToken token)
    {
        _logger.LogDebug($"{nameof(Subscribe)}({typeof(THandler).Name}) start...");
        var handler = _sp.GetRequiredService<THandler>();
        _handlers.Add(handler);
        
        ProcessSubscription(token, handler);

        await Task.Run(() =>
        {
            while (_isActive)
                Thread.Sleep(1000);
        });
    }

    private void ProcessSubscription(CancellationToken token, THandler handler)
    {
        using var connection = _rabbitConnectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        var queue = GetRequestQueueName();
        var declareResult = _settings.QueueSettings.TryCreate ? channel.QueueDeclare(queue, _settings.QueueSettings.Durable, exclusive: false) : channel.QueueDeclarePassive(queue);

        _logger.LogDebug($"{nameof(Subscribe)}({typeof(THandler).Name}) queue declare: {declareResult.QueueName}");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            try
            {
                _logger.LogDebug($"{nameof(Subscribe)}() message received");

                var deserialized = JsonSerializer.Deserialize<SendMessageRequest>(ea.Body.ToArray());
                var policy = Policy.Handle<Exception>()
                                   .WaitAndRetry(3, n => TimeSpan.FromSeconds(0.5 * Math.Exp(n)));

                policy.Execute(() => handler.Handle(deserialized, token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        };

        channel.BasicConsume(queue,
                             true,
                             consumer);
    }

    private async Task InnerSend(SendMessageResponse response)
    {
        try
        {
            using var connection = _rabbitConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var rk = GetRkName();
            var queue = GetResponseQueueName();

            _ = _settings.QueueSettings is {TryCreate: true, CheckQueueOnPublish: true} ?
                    channel.QueueDeclare(queue, _settings.QueueSettings.Durable, false) :
                    channel.QueueDeclarePassive(queue);

            channel.QueueBind(queue, _settings.Exchange, rk);
            channel.BasicPublish(_settings.Exchange, rk, body: JsonSerializer.SerializeToUtf8Bytes(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            throw;
        }
    }
}