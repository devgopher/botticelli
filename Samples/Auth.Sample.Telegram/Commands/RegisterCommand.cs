using Botticelli.Framework.Commands;

namespace Auth.Sample.Telegram.Commands;

/// <summary>
///     Registers a user
/// </summary>
public class RegisterCommand : ICommand
{
    public Guid Id { get; }
}