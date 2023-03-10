using System.Text.Json;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Bus.Rabbit.Exceptions;
using Botticelli.Bus.Rabbit.Settings;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Botticelli.Bus.Rabbit.Client;

public class RabbitClient<TBot> : BasicFunctions<TBot>, IBotticelliBusClient
        where TBot : IBot
{
    private readonly ILogger<RabbitClient<TBot>> _logger;
    private readonly IConnectionFactory _rabbitConnectionFactory;
    private readonly Dictionary<string, SendMessageResponse> _responses = new(100);
    private readonly RabbitBusSettings _settings;
    private readonly TimeSpan _timeout = TimeSpan.FromMinutes(1);
    private readonly int delta = 50;
    private EventingBasicConsumer? _consumer = default;

    public RabbitClient(TBot bot,
                        IConnectionFactory rabbitConnectionFactory,
                        RabbitBusSettings settings,
                        ILogger<RabbitClient<TBot>> logger)
    {
        _rabbitConnectionFactory = rabbitConnectionFactory;
        _settings = settings;
        _logger = logger;

        Init();
    }

    public async Task<SendMessageResponse> GetResponse(SendMessageRequest request, CancellationToken token, int timeoutMs = 60000)
    {
        try
        {
            using var connection = _rabbitConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            Send(request, channel, GetRequestQueueName());

            var result = new Task<SendMessageResponse>(request =>
                                                       {
                                                           var req = request as SendMessageRequest;
                                                           var dt = DateTime.Now;

                                                           while (!_responses.ContainsKey(req.Message.Uid))
                                                           {
                                                               Thread.Sleep(delta);

                                                               if (DateTime.Now - dt >= _timeout) throw new TimeoutException("Timeout");
                                                           }
                                                           
                                                           var resp = _responses[req.Message.Uid];
                                                           _responses.Remove(req.Message.Uid);

                                                           return resp;
                                                       },
                                                       request);

            result.Start();

            if (result.IsFaulted)
                throw new RabbitBusException($"Error getting a response: {result.Exception.Message}",
                                             result.Exception?.InnerException);

            return await result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            throw;
        }
    }

    public async Task SendResponse(SendMessageResponse response, CancellationToken token)
    {
        try
        {
            using var connection = _rabbitConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();


            Send(response, channel, GetResponseQueueName());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            throw;
        }
    }

    private void Init()
    {
        var connection = _rabbitConnectionFactory.CreateConnection();
        var channel = connection.CreateModel();

        _consumer ??= new EventingBasicConsumer(channel);
        var queue = GetResponseQueueName();
        var exchange = _settings.Exchange;

        if (_settings.QueueSettings.TryCreate)
            channel.ExchangeDeclare(exchange, "direct");
        else
            channel.ExchangeDeclarePassive(exchange);

        var queueDeclareResult = _settings.QueueSettings.TryCreate ? channel.QueueDeclare(queue, _settings.QueueSettings.Durable, exclusive:false) : channel.QueueDeclarePassive(queue);


        channel.BasicConsume(queue, true, _consumer);

        _consumer.Received += (sender, args) =>
        {
            if (args?.Body == null) return;

            var response = JsonSerializer.Deserialize<SendMessageResponse>(args.Body.ToArray());

            if (response == null) return;

            _responses.Add(response?.Message.Uid, response);
        };
    }

    private void Send(object input, IModel channel, string queue)
    {
        _ = _settings.QueueSettings is {TryCreate: true, CheckQueueOnPublish: true} ?
                channel.QueueDeclare(queue, _settings.QueueSettings.Durable, false) :
                channel.QueueDeclarePassive(queue);

        channel.QueueBind(queue, _settings.Exchange, queue);
        channel.BasicPublish(_settings.Exchange, queue, body: JsonSerializer.SerializeToUtf8Bytes(input));
    }
}