using Botticelli.Framework.Commands;

namespace Broadcasting.Sample.Common.Commands;

public class StartCommand : ICommand
{
    public Guid Id { get; }
}