using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bot.Interfaces.Agent;

/// <summary>
/// Bus agent works on the side of a endpoint
/// </summary>
public interface IBotticelliBusAgent
{
    public Task<SendMessageResponse> SendResponse(SendMessageRequest request,
                                                  CancellationToken token,
                                                  int timeoutMs = 10000);
}