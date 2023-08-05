using Botticelli.Framework.Commands;

namespace TelegramMessagingSample.Commands;

public class StartCommand : ICommand
{
    public Guid Id { get; }
}