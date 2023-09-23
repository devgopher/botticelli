using MediatR;

namespace Botticelli.Framework.Events;

public class MessageRemovedBotEventArgs : BotEventArgs, IRequest
{
    public string? MessageUid { get; set; }
}