using Botticelli.Framework.Commands;

namespace MessagingSample.Common.Commands;

public class StopCommand : ICommand
{
    public Guid Id { get; }
}