using Botticelli.Shared.ValueObjects;

namespace Botticelli.Framework.Events;

public class SharedContactBotEventArgs : BotEventArgs
{
    public required Contact Contact { get; set; }
}