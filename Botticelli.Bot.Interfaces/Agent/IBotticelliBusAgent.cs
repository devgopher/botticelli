using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Bot.Interfaces.PubSub;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bot.Interfaces.Agent;

/// <summary>
/// Bus agent works on the side of a endpoint
/// </summary>
public interface IBotticelliBusAgent<in THandler> : ISubscriber<THandler, SendMessageResponse> 
        where THandler : IHandler<SendMessageResponse>
{
    public Task<SendMessageResponse> SendResponse(SendMessageRequest request,
                                                  CancellationToken token,
                                                  int timeoutMs = 10000);
}