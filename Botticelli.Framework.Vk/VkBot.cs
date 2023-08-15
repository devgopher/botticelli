using BotDataSecureStorage;
using Botticelli.BotBase.Utils;
using Botticelli.Framework.Exceptions;
using Botticelli.Framework.Vk.Options;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.Framework.Vk
{
    public class VkBot : BaseBot<VkBot>
    {
        private readonly LongPollMessagesProvider _messagesProvider;
        private readonly MessagePublisher _messagePublisher;
        private readonly SecureStorage _secureStorage;
        private readonly IOptionsMonitor<VkBotSettings> _settings;

        public VkBot(LongPollMessagesProvider messagesProvider,
                     MessagePublisher messagePublisher,
                     SecureStorage secureStorage,
                     ILogger<VkBot> logger,
                     IOptionsMonitor<VkBotSettings> settings) : base(logger)
        {
            _messagesProvider = messagesProvider;
            _messagePublisher = messagePublisher;
            _secureStorage = secureStorage;
            _settings = settings;
        }


        [Obsolete($"Use {nameof(SetBotContext)}")]
        public override async Task SetBotKey(string key, CancellationToken token)
            => _messagesProvider.SetApiKey(key);

        public override async Task SetBotContext(BotContext context, CancellationToken token)
        {
            var currentContext = _secureStorage.GetBotContext(BotDataUtils.GetBotId());

            if (currentContext?.BotKey != context.BotKey)
            {
                var stopRequest = StopBotRequest.GetInstance();
                var startRequest = StartBotRequest.GetInstance();
                await StopBotAsync(stopRequest, token);

                _secureStorage.SetBotContext(context);
                _messagesProvider.SetApiKey(context.BotKey);
                _messagePublisher.SetApiKey(context.BotKey);
                await StartBotAsync(startRequest, token);
            }
        }

        public override async Task<SendMessageResponse> SendMessageAsync<TSendOptions>(SendMessageRequest request,
                                                                                       ISendOptionsBuilder<TSendOptions> optionsBuilder, 
                                                                                       CancellationToken token)
        {
            try
            {;
                var currentContext = _secureStorage.GetBotContext(BotDataUtils.GetBotId());

                foreach (var userId in request.Message.ChatIds)
                {

                    await _messagePublisher.SendAsync(new API.Requests.SendMessageRequest()
                    {
                        AccessToken = currentContext.BotKey,
                        UserId = userId,
                        Body = request.Message.Body
                    }, token);
                }
            }
            catch (Exception ex)
            {
                throw new BotException($"Can't send a message!", ex);
            }
        }

        public override async Task<RemoveMessageResponse> DeleteMessageAsync(RemoveMessageRequest request, CancellationToken token) => throw new NotImplementedException();

        public override BotType Type { get; }
        public override event MsgReceivedEventHandler MessageReceived;
        public override event MsgRemovedEventHandler MessageRemoved;
        public override event MessengerSpecificEventHandler MessengerSpecificEvent;
    }
}
