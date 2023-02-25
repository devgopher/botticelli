using Botticelli.AI.Message;

namespace Botticelli.AI.AIProvider;

public interface IAiProvider
{
    public string AiName { get; }
    public Task SendAsync(AiMessage message, CancellationToken token);
}