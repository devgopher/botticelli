using Botticelli.Framework.Commands;
using Botticelli.Shared.ValueObjects;

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
    /// <returns></returns>
    public Task<string> ThrowCommand(ICommand command);

    /// <summary>
    /// Waits response for a command
    /// </summary>
    /// <param name="messageUid">Tracked message uid</param>
    /// <returns></returns>
    public Task<Message> WaitForResponse(string messageUid, TimeSpan timeout);
}



