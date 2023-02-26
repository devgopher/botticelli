namespace Botticelli.Framework.Commands.Validators;

public class PassValidator<TCommand> : ICommandValidator<TCommand>
        where TCommand : ICommand
{
    public async Task<bool> Validate(string chatId, string args) => true;

    public string Help() => string.Empty;
}