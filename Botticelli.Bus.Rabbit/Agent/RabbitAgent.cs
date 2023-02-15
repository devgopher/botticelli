using System.Text.Json;
using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Bus.Rabbit.Settings;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using RabbitMQ.Client;

namespace Botticelli.Bus.Rabbit.Agent
{
    /// <summary>
    /// RabbitMQ agent
    /// </summary>
    /// <typeparam name="TBot"/>
    public class RabbitAgent<TBot, THandler> : IBotticelliBusAgent<THandler>
            where TBot : IBot where THandler : IHandler<SendMessageResponse>
    {
        private readonly TBot _bot;
        private readonly IConnectionFactory _rabbitConnectionFactory;
        private readonly RabbitBusSettings _settings;

        public RabbitAgent(TBot bot, IConnectionFactory rabbitConnectionFactory, RabbitBusSettings settings)
        {
            _bot = bot;
            _rabbitConnectionFactory = rabbitConnectionFactory;
            _settings = settings;
        }

        public async Task<SendMessageResponse> SendResponse(SendMessageRequest request, 
                                                            CancellationToken token,
                                                            int timeoutMs = 10000)
        {
            using var connection = _rabbitConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(GetQueueName(request));
            var rk = GetRkName(request);
            var queue = GetQueueName(request);

            channel.QueueBind(queue, _settings.Exchange, rk);
            channel.BasicPublish(_settings.Exchange, rk, body: JsonSerializer.SerializeToUtf8Bytes(request));

            return await _rabbitConnectionFactory.;
        }

        private static string GetQueueName(SendMessageRequest request) => $"{nameof(SendMessageRequest)}_{request.Message?.ChatId}";
        private static string GetRkName(SendMessageRequest request) => $"{nameof(SendMessageRequest)}_{request.Message?.ChatId}_rk";

        public void Subscribe(THandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
