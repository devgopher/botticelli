using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.WhatsApp;

public class WhatsAppBot : BaseBot<WhatsAppBot>
{
    private readonly ILogger<WhatsAppBot> _logger;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="logger"></param>
    public WhatsAppBot(ILogger<WhatsAppBot> logger) : base(logger)
    {
        IsStarted = false;
        _logger = logger;
    }

    public override async Task<SendMessageResponse> SendMessageAsync<TSendOptions>(SendMessageRequest request, SendOptionsBuilder<TSendOptions> optionsBuilder, CancellationToken token)
        => throw new NotImplementedException();

    public override async Task<RemoveMessageResponse> DeleteMessageAsync(RemoveMessageRequest request, CancellationToken token) => throw new NotImplementedException();

    public override BotType Type => BotType.Telegram;
    public virtual event MsgSentEventHandler MessageSent;
    public override event MsgReceivedEventHandler MessageReceived;
    public override event MsgRemovedEventHandler MessageRemoved;
    public override event MessengerSpecificEventHandler MessengerSpecificEvent;
}