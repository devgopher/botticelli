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
    private bool? _isActive;

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
            _logger.LogDebug("{SendResponseAsyncName}({ResponseUid}) start...", nameof(SendResponseAsync), response.Uid);

            var policy = Policy.Handle<RabbitMQClientException>()
                               .WaitAndRetryAsync(5, n => TimeSpan.FromSeconds(3 * Math.Exp(n)));

            await policy.ExecuteAsync(() => InnerSend(response));

            _logger.LogDebug("{SendResponseAsyncName}({ResponseUid}) finished", nameof(SendResponseAsync), response.Uid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending a response: {ExMessage}", ex.Message);
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _isActive = true;
        await Subscribe(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _isActive = false;
        Thread.Sleep(3000);

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Subscribes with a new handler
    /// </summary>
    public Task Subscribe(CancellationToken token)
    {
        _logger.LogDebug("{SubscribeName}({Name}) start...", nameof(Subscribe), typeof(THandler).Name);
        var handler = _sp.GetRequiredService<THandler>();

        ProcessSubscription(token, handler);

        return Task.CompletedTask;
    }

    private void ProcessSubscription(CancellationToken token, THandler handler)
    {
        if (_consumer == null)
        {
            var connection = _rabbitConnectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            var queue = GetRequestQueueName();
            var declareResult = _settings.QueueSettings is {TryCreate: true} ? channel.QueueDeclare(queue, _settings.QueueSettings.Durable, false) : channel.QueueDeclarePassive(queue);

            _logger.LogDebug("{SubscribeName}({Name}) queue declare: {DeclareResultQueueName}", nameof(Subscribe), typeof(THandler).Name, declareResult.QueueName);

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

                if (deserialized != null) policy.Execute(() => handler.Handle(deserialized, token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        };
    }

    private Task InnerSend(SendMessageResponse response)
    {
        try
        {
            using var connection = _rabbitConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var rk = GetResponseQueueName();
            var queue = GetResponseQueueName();

            _ = _settings.QueueSettings is {TryCreate: true, CheckQueueOnPublish: true} ? channel.QueueDeclare(queue, _settings.QueueSettings.Durable, false) : channel.QueueDeclarePassive(queue);

            channel.QueueBind(queue, _settings.Exchange, rk);
            channel.BasicPublish(_settings.Exchange, rk, body: JsonSerializer.SerializeToUtf8Bytes(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            throw;
        }

        return Task.CompletedTask;
    }
}