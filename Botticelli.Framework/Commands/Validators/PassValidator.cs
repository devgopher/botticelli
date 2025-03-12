using Botticelli.Shared.ValueObjects;

namespace Botticelli.Framework.Commands.Validators;

public class PassValidator<TCommand> : ICommandValidator<TCommand>
        where TCommand : ICommand
{
    public Task<bool> Validate(Message message)
    {
        return Task.FromResult(true);
    }

    public string Help()
    {
        return string.Empty;
    }
}