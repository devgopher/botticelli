using Botticelli.Shared.API.Client.Commands;

namespace Botticelli.Shared.ValueObjects;

/// <summary>
///     A message with command
/// </summary>
public class CommandMessage : Message
{
    protected CommandMessage(string uid) : base(uid)
    {
    }

    /// <summary>
    ///     Command
    /// </summary>
    public IBotCommand Command { get; set; }
}