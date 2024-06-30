using Botticelli.Bot.Interfaces.Processors;
using Botticelli.LoadTests.Receiver.Result;

namespace Botticelli.LoadTests.Receiver;

/// <summary>
/// A load tests gate allows access of a load test tool to bot's commands
/// </summary>
public interface ILoadTestGate
{
    /// <summary>
    /// Throws a command and returns a message uid in order to track a response
    /// </summary>
    /// <param name="command"></param>
    /// <param name="args"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task ThrowCommand(string command, string args, CancellationToken token);

    /// <summary>
    /// Waits response for a command
    /// </summary>
    /// <param name="task"></param>
    /// <param name="timeout"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<CommandResult> WaitForExecution(Task task, TimeSpan timeout, CancellationToken token);
}