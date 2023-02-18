namespace Botticelli.Framework.Commands.Validators;

public class PassValidator<TCommand> : ICommandValidator<TCommand>
        where TCommand : ICommand
{
    public string Help() => string.Empty;

    public async Task<bool> Validate(long chatId, params string[] args) => true;
}