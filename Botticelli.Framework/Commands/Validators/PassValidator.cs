namespace Botticelli.Framework.Commands.Validators;

public class PassValidator<TCommand> : ICommandValidator<TCommand>
    where TCommand : ICommand
{
    public async Task<bool> Validate(List<string> chatId, string body) => true;

    public string Help() => string.Empty;
}