using Botticelli.Framework.Commands;

namespace MessagingSample.Common.Commands;

public class InfoCommand : ICommand
{
    public Guid Id { get; }
}