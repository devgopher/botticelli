using MediatR;

namespace Botticelli.Framework.Events;

public class MessageRemovedBotEventArgs : BotEventArgs, IRequest, INotification
{
    public string? MessageUid { get; set; }
}