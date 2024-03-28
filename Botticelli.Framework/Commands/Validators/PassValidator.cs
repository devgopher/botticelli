namespace Botticelli.Framework.Commands.Validators;

public class PassValidator<TCommand> : ICommandValidator<TCommand>
    where TCommand : ICommand
{
    public Task<bool> Validate(List<string> chatId, string args) => Task.FromResult(true);

    public string Help() => string.Empty;
}