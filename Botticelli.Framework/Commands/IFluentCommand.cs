namespace Botticelli.Framework.Commands;

public interface IFluentCommand : ICommand
{
    public static abstract string? CommandName { get; }
}