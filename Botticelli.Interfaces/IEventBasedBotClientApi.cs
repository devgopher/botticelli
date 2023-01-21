using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Interfaces;

public interface IEventBasedBotClientApi
{
    public Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken token);
    public Task<RemoveMessageResponse> DeleteMessageAsync(RemoveMessageRequest request, CancellationToken token);
}