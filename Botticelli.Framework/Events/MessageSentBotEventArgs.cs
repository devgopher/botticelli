using Botticelli.Shared.ValueObjects;
using MediatR;

namespace Botticelli.Framework.Events;

public class MessageSentBotEventArgs : BotEventArgs, IRequest
{
    public Message Message { get; set; }
}