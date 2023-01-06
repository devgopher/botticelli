using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Interfaces;

public interface IEventBasedBotClientApi
{
    public Task<SendMessageResponse> SendAsync(SendMessageRequest request);

    public void AddClientEventProcessor(IClientResponseProcessor responseProcessor);
}