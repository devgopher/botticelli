using BotDataSecureStorage;
using Botticelli.BotBase.Utils;
using Botticelli.Framework.Exceptions;
using Botticelli.Framework.Vk.API.Responses;
using Botticelli.Framework.Vk.Handlers;
using Botticelli.Framework.Vk.API.Objects;
using Botticelli.Framework.Vk.Options;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Vk
{
    public class VkBot : BaseBot<VkBot>
    {
        private readonly LongPollMessagesProvider _messagesProvider;
        private readonly MessagePublisher _messagePublisher;
        private readonly SecureStorage _secureStorage;
        private readonly IBotUpdateHandler _handler;
        private bool _eventsAttached = false;

        public VkBot(LongPollMessagesProvider messagesProvider,
                     MessagePublisher messagePublisher,
                     SecureStorage secureStorage,
                     IBotUpdateHandler handler,
                     ILogger<VkBot> logger) : base(logger)
        {
            _messagesProvider = messagesProvider;
            _messagePublisher = messagePublisher;
            _secureStorage = secureStorage;
            _handler = handler;
        }


        [Obsolete($"Use {nameof(SetBotContext)}")]
        public override async Task SetBotKey(string key, CancellationToken token)
            => _messagesProvider.SetApiKey(key);
        
        public override async Task<StartBotResponse> StartBotAsync(StartBotRequest request, CancellationToken token)
        {
            try
            {
                Logger.LogInformation($"{nameof(StartBotAsync)}...");
                var response = await base.StartBotAsync(request, token);

                if (IsStarted)
                {
                    Logger.LogInformation($"{nameof(StartBotAsync)}: already started");

                    return response;
                }

                if (response.Status != AdminCommandStatus.Ok || IsStarted) return response;

                if (!_eventsAttached)
                {
                    _messagesProvider.OnUpdates += (args, token) =>
                    {
                        var updates = args?.Response?.Updates;

                        if (updates == default || !updates.Any()) return;

                        _handler.HandleUpdateAsync(updates, CancellationToken.None);
                    };
                    _eventsAttached = true;
                }

                await _messagesProvider.Start(token);

                IsStarted = true;
                Logger.LogInformation($"{nameof(StartBotAsync)}: started");

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }

            return StartBotResponse.GetInstance(AdminCommandStatus.Fail, "error");
        }

        public override async Task SetBotContext(BotContext context, CancellationToken token)
        {
            var currentContext = _secureStorage.GetBotContext(BotDataUtils.GetBotId());

            if (currentContext?.BotKey != context.BotKey)
            {
                var stopRequest = StopBotRequest.GetInstance();
                var startRequest = StartBotRequest.GetInstance();
                await StopBotAsync(stopRequest, token);

                _secureStorage.SetBotContext(context);

                await _messagesProvider.Stop();
                _messagesProvider.SetApiKey(context.BotKey);
                _messagePublisher.SetApiKey(context.BotKey);

                await _messagesProvider.Start(token);
                await StartBotAsync(startRequest, token);
            }

            _messagesProvider.SetApiKey(context.BotKey);
            _messagePublisher.SetApiKey(context.BotKey);
        }

        private API.Objects.Attachment CreateVkAttach(IAttachment fk, string type) 
            => new API.Objects.Attachment()
        {
            MediaId = fk.Uid,
            OwnerId = fk.OwnerId,
            Type = type
        };

        public override async Task<SendMessageResponse> SendMessageAsync<TSendOptions>(SendMessageRequest request,
                                                                                       ISendOptionsBuilder<TSendOptions> optionsBuilder, 
                                                                                       CancellationToken token)
        {
            try
            {
                var currentContext = _secureStorage.GetBotContext(BotDataUtils.GetBotId());

                foreach (var userId in request.Message.ChatIds)
                {
                    var attachmentsCount = request.Message.Attachments?.Count ?? 0;
                    API.Objects.Attachment vkAttach = null;
                    
                    if (attachmentsCount > 0)
                    {
                        var fk = request.Message.Attachments?.FirstOrDefault();
                        vkAttach = CreateAttach(fk);
                    }

                    var vkRequest = new API.Requests.SendMessageRequest()
                    {
                        AccessToken = currentContext.BotKey,
                        UserId = userId,
                        Body = request.Message.Body,
                        Lat = request.Message.Location?.Latitude,
                        Long = request.Message.Location?.Longitude,
                        ReplyTo = request.Message.ReplyToMessageUid,
                        Attachment = vkAttach
                    };

                    await _messagePublisher.SendAsync(vkRequest, token);

                    // If we need to send another attaсhs - let's send then separately!
                    if (attachmentsCount > 1)
                    {
                        foreach (var fk in request.Message.Attachments.Skip(1))
                        {
                            vkAttach = CreateAttach(fk);

                            var vkAttachRequest = new API.Requests.SendMessageRequest()
                            {
                                AccessToken = currentContext.BotKey,
                                UserId = userId,
                                Body = string.Empty,
                                Lat = request.Message.Location?.Latitude,
                                Long = request.Message.Location?.Longitude,
                                ReplyTo = request.Message.ReplyToMessageUid,
                                Attachment = vkAttach
                            };

                            await _messagePublisher.SendAsync(vkAttachRequest, token);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BotException($"Can't send a message!", ex);
            }

            return new SendMessageResponse(request.Uid, string.Empty);
        }

        private Attachment CreateAttach(IAttachment? fk)
        {
            switch (fk)
            {
                case BinaryAttachment ba:
                    {
                        switch (ba.MediaType)
                        {
                            case MediaType.Image:
                            case MediaType.Sticker:
                                return CreateVkAttach(fk, "photo");
                            case MediaType.Video:
                                return CreateVkAttach(fk, "video");
                            case MediaType.Voice:
                            case MediaType.Audio:
                                return CreateVkAttach(fk, "audio");
                            default: break;
                        }
                    }
                    break;
                case InvoiceAttachment:
                    // Not implemented 
                    break;
            }

            return default;
        }

        public override async Task<RemoveMessageResponse> DeleteMessageAsync(RemoveMessageRequest request, CancellationToken token) => throw new NotImplementedException();

        public override BotType Type => BotType.Vk;
        public override event MsgReceivedEventHandler MessageReceived;
        public override event MsgRemovedEventHandler MessageRemoved;
        public override event MessengerSpecificEventHandler MessengerSpecificEvent;
    }
}
