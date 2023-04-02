using Botticelli.Shared.ValueObjects;

namespace Botticelli.Framework.Events;

public class MessageSentBotEventArgs : BotEventArgs
{
    public Message Message { get; set; }
}