using Botticelli.Framework.Exceptions;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.DependencyInjection;
using Viber.Api;
using Viber.Api.Entities;
using Viber.Api.Requests;

namespace Botticelli.Framework.Viber
{
    public class ViberBot : BaseBot<ViberBot>//, IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly IViberService _viberService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="client"></param>
        public ViberBot(IViberService viberService)
        {
        //   _scope = services.BuildServiceProvider().CreateScope();
        //   _viberService = _scope.ServiceProvider.GetRequiredService<IViberService>();
        //   _viberService.SetWebhook(new SetWebhookRequest()
        //   {

        //   })
        //
        _viberService = viberService;
        }

        /// <summary>
        /// Sends a message as a telegram bot
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="BotException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override async Task<SendMessageResponse> SendAsync(SendMessageRequest request, CancellationToken token)
        {
            SendMessageResponse response = new(request.Uid, string.Empty);
            ApiSendMessageRequest messageRequest;

            try
            {
                if (request?.Message == default)
                    throw new BotException("request/message is null!");
                
                var text = $"{request.Message.Subject} {request.Message.Body}";

                if (!string.IsNullOrWhiteSpace(text))
                {
                    messageRequest = new ApiSendMessageRequest()
                    {
                        Sender = new Sender
                        {
                            Name = string.Empty,
                            Avatar = null
                        },
                        Receiver = request.Message.ChatId,
                        MinApiVersion = 0,
                        TrackingData = null,
                        Text = text
                    };

                    await _viberService.SendMessage(messageRequest, token);
                }

                // Only text... temporary
                //if (request.Message.Attachments != null)
                //{
                //    foreach (var attachment in request.Message.Attachments)
                //    {
                //        switch (attachment.MediaType)
                //        {
                //            case MediaType.Audio:
                //                // nothing to do
                //                break;
                //            case MediaType.Video:
                //                break;
                //            case MediaType.Image:
   
                //                break;
                //            case MediaType.Voice:
                //                break;
                //            case MediaType.Text:
                //                break;
                //            default:
                //                throw new ArgumentOutOfRangeException();
                //        }
                //    }
                //}
                
                response.MessageSentStatus = MessageSentStatus.OK;
            }
            catch (Exception ex)
            {
                response.MessageSentStatus = MessageSentStatus.FAIL;
            }

            return response;
        }

        public override async Task<StartBotResponse> StartBotAsync(StartBotRequest request, CancellationToken token)
        {
            var response = await base.StartBotAsync(request, token);

            //if (response.Status == AdminCommandStatus.OK)
            //    _client.StartReceiving(_sp.GetRequiredService<IUpdateHandler>(), cancellationToken: token);

            return response;
        }

        public override async Task<StopBotResponse> StopBotAsync(StopBotRequest request, CancellationToken token)
        {
            var response = await base.StopBotAsync(request, token);

            //if (response.Status == AdminCommandStatus.OK)
            //    await _client.CloseAsync(cancellationToken: token);

            return response;
        }

        //public void Dispose() => _scope.Dispose();
    }
}
