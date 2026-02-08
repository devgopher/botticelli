namespace Botticelli.Framework.Commands;

public interface ICommand
{
    Guid Id { get; }
    string CommandText => GetType().Name.Replace("Command", string.Empty);
}