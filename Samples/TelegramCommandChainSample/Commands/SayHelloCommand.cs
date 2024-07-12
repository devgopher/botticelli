using Botticelli.Framework.Commands;

namespace TelegramCommandChainSample.Commands;

public class SayHelloCommand : ICommand
{
    public Guid Id { get; }
}