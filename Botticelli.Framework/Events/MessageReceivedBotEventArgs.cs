using Botticelli.Shared.ValueObjects;

namespace Botticelli.Framework.Events;

public class MessageReceivedBotEventArgs : BotEventArgs
{
    public Message Message { get; set; }
}