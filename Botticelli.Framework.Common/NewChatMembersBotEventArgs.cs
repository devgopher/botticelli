using Botticelli.Shared.ValueObjects;

namespace Botticelli.Framework.Events;

public class NewChatMembersBotEventArgs : BotEventArgs
{
    public required User User { get; set; }
}