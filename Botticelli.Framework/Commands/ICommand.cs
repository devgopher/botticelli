namespace Botticelli.Framework.Commands;

public interface ICommand
{
    Guid Id { get; }
    static string CommandName => GetType().Name.Replace("Command", string.Empty);
}