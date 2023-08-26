using Botticelli.Framework.Commands;

namespace VkMessagingSample.Commands;

public class StartCommand : ICommand
{
    public Guid Id { get; }
}