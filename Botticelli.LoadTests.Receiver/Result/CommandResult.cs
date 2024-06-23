using Botticelli.Shared.ValueObjects;

namespace Botticelli.LoadTests.Receiver.Result;

/// <summary>
/// Load test command result
/// </summary>
public class CommandResult
{
    /// <summary>
    /// Response from a bot
    /// </summary>
    public Message ResultMessage { get; set; }

    /// <summary>
    /// Elapsed time
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Successful or not
    /// </summary>
    public bool IsSuccess { get; set; }
}