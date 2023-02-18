using Botticelli.AI.Message;

namespace Botticelli.AI.AIProvider;

public interface IAiProvider
{
    public Task SendAsync(AiMessage message, CancellationToken token);
}