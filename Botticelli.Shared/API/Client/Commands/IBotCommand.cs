namespace Botticelli.Shared.API.Client.Commands;

/// <summary>
///     Bot command with name and it's parameters
/// </summary>
public interface IBotCommand
{
    /// <summary>
    ///     Command name
    /// </summary>
    public string CommandName { get; set; }

    /// <summary>
    ///     Command params
    /// </summary>
    public IEnumerable<BotCommandParameter> Parameters { get; set; }
}