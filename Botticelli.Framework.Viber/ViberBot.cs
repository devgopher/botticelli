using Botticelli.Framework.Exceptions;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Viber.Api;
using Viber.Api.Entities;
using Viber.Api.Requests;

namespace Botticelli.Framework.Viber;

public class ViberBot : BaseBot<ViberBot> //, IDisposable
{
    private readonly IViberService _viberService;
    //private readonly ViberBotSettings _settings;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="viberService"></param>
    /// <param name="settings"></param>
    public ViberBot(IViberService viberService /*, ViberBotSettings setting*/) => _viberService = viberService;

    public override BotType Type => BotType.Viber;

    //_settings = settings;
    public override Task<RemoveMessageResponse> DeleteMessageAsync(RemoveMessageRequest request, CancellationToken token) => throw new NotImplementedException();

    /// <summary>
    ///     Sends a message as a telegram bot
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="BotException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override async Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken token)
    {
        SendMessageResponse response = new(request.Uid, string.Empty);
        ApiSendMessageRequest messageRequest;

        try
        {
            if (request?.Message == default) throw new BotException("request/message is null!");

            var text = $"{request.Message.Subject} {request.Message.Body}";

            if (!string.IsNullOrWhiteSpace(text))
            {
                messageRequest = new ApiSendMessageRequest
                {
                    Sender = new Sender
                    {
                        Name = string.Empty,
                        Avatar = null
                    },
                    Receiver = request.Message.ChatId,
                    MinApiVersion = 0,
                    TrackingData = null,
                    Text = text,
                    Type = "text"
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

            response.MessageSentStatus = MessageSentStatus.Ok;
        }
        catch (Exception ex)
        {
            response.MessageSentStatus = MessageSentStatus.Fail;
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