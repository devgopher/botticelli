using System.Text.Json;
using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Bus.Handlers;
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
    private readonly ILogger<RabbitAgent<TBot, THandler>> _logger;
    private readonly IConnectionFactory _rabbitConnectionFactory;
    private readonly RabbitBusSettings _settings;
    private readonly IServiceProvider _sp;
    private EventingBasicConsumer? _consumer;
    private bool _isActive;

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
    public async Task SendResponseAsync(SendMessageResponse response,
        CancellationToken token,
        int timeoutMs = 60000)
    {
        try
        {
            _logger.LogDebug($"{nameof(SendResponseAsync)}({response.Uid}) start...");

            var policy = Policy.Handle<RabbitMQClientException>()
                .WaitAndRetryAsync(5, n => TimeSpan.FromSeconds(3 * Math.Exp(n)));

            await policy.ExecuteAsync(() => InnerSend(response));

            _logger.LogDebug($"{nameof(SendResponseAsync)}({response.Uid}) finished");
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

        ProcessSubscription(token, handler);
    }

    private void ProcessSubscription(CancellationToken token, THandler handler)
    {
        if (_consumer == default)
        {
            var connection = _rabbitConnectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            var queue = GetRequestQueueName();
            var declareResult = _settings.QueueSettings.TryCreate
                ? channel.QueueDeclare(queue, _settings.QueueSettings.Durable, false)
                : channel.QueueDeclarePassive(queue);

            _logger.LogDebug($"{nameof(Subscribe)}({typeof(THandler).Name}) queue declare: {declareResult.QueueName}");

            _consumer = new EventingBasicConsumer(channel);


            channel.BasicConsume(queue,
                true,
                _consumer);
        }

        _consumer.Received += (model, ea) =>
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
    }

    private async Task InnerSend(SendMessageResponse response)
    {
        try
        {
            using var connection = _rabbitConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var rk = GetResponseQueueName();
            var queue = GetResponseQueueName();

            _ = _settings.QueueSettings is { TryCreate: true, CheckQueueOnPublish: true }
                ? channel.QueueDeclare(queue, _settings.QueueSettings.Durable, false)
                : channel.QueueDeclarePassive(queue);

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