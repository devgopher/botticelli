using Botticelli.Shared.ValueObjects;
using MediatR;

namespace Botticelli.Framework.Events;

public class MessageReceivedBotEventArgs : BotEventArgs, IRequest
{
    public Message Message { get; set; }
}