namespace Botticelli.LoadTests.Receiver.Result;

/// <summary>
///     Load test command result
/// </summary>
public class CommandResult
{
    /// <summary>
    ///     Result string
    /// </summary>
    public string? ResultMessage { get; set; }

    /// <summary>
    ///     Elapsed time
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    ///     Successful or not
    /// </summary>
    public bool IsSuccess { get; set; }
}