using Botticelli.Framework.Events;
using Botticelli.Framework.Exceptions;
using Botticelli.Framework.Viber.WebHook;
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

    private readonly INotificator<ViberBot> _notificator;
    //private readonly ViberBotSettings _settings;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="viberService"></param>
    /// <param name="settings"></param>
    /// <param name="notificator"></param>
    public ViberBot(IViberService viberService, INotificator<ViberBot> notificator /*, ViberBotSettings setting*/)
    {
        _viberService = viberService;
        _notificator = notificator;
    }

    public override BotType Type => BotType.Viber;

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

                MessageSent?.Invoke(this, new MessageSentBotEventArgs
                {
                    Message = request?.Message
                });
            }

            response.MessageSentStatus = MessageSentStatus.Ok;
        }
        catch (Exception ex)
        {
            response.MessageSentStatus = MessageSentStatus.Fail;
        }

        return response;
    }

    public override event MsgSentEventHandler MessageSent;
    public override event MsgReceivedEventHandler MessageReceived
    {
        add => _notificator.MessageReceived += value.Invoke;
        remove => _notificator.MessageReceived -= value.Invoke;
    }

    public override event MsgRemovedEventHandler MessageRemoved;
    public override event MessengerSpecificEventHandler MessengerSpecificEvent
    {
        add => _notificator.MessengerSpecificEvent += value.Invoke;
        remove => _notificator.MessengerSpecificEvent -= value.Invoke;
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