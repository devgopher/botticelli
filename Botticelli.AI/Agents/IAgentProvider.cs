using Botticelli.AI.Message;

namespace Botticelli.AI.Agents;

/// <summary>
/// Defines a contract for AI agent providers that process user messages and return AI-generated responses.
/// Implementations send messages to a specific AI backend (e.g. YaGpt) and deliver results via the bus.
/// </summary>
public interface IAgentProvider
{
    /// <summary>
    /// Sends the given AI message to the agent for processing and delivers the response asynchronously.
    /// </summary>
    /// <param name="inputMessage">The message to send to the AI agent.</param>
    /// <param name="token">Cancellation token for the operation.</param>
    Task SendAsync(AiMessage inputMessage, CancellationToken token);

    /// <summary>
    /// Display name of the AI provider (e.g. "YaGpt") used for logging and error messages.
    /// </summary>
    string AiName { get; }
}