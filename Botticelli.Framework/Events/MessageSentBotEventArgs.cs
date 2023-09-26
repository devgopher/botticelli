using Botticelli.Shared.ValueObjects;
using MediatR;

namespace Botticelli.Framework.Events;

public class MessageSentBotEventArgs : BotEventArgs, IRequest, INotification
{
    public Message Message { get; set; }
}