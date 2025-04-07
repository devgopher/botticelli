using Botticelli.Framework.Commands;

namespace Auth.Sample.Telegram.Commands;

public class StartCommand : ICommand
{
    public Guid Id { get; }
}