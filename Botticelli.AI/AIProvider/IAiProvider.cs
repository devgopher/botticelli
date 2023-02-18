using Botticelli.AI.Message;

namespace Botticelli.AI.AIProvider;

public interface IAiProvider
{
    public Task SendAsync(AIMessage message, CancellationToken token);
}