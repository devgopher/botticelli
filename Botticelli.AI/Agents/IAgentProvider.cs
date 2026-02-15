using Botticelli.AI.Message;

namespace Botticelli.AI.Agents;

/// <summary>
/// AI agent provider
/// </summary>
public interface IAgentProvider
{
    Task SendAsync(AiMessage inputMessage, CancellationToken token);
    
    public abstract string AiName { get; }
}