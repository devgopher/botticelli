using Botticelli.Framework.Commands;

namespace MessagingSample.Common.Commands;

public class StartCommand : ICommand
{
    public Guid Id { get; }
}