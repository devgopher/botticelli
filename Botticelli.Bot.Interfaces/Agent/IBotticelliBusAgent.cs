using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Bot.Interfaces.PubSub;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bot.Interfaces.Agent;

/// <summary>
///     Bus agent works on the side of a endpoint
/// </summary>
public interface IBotticelliBusAgent
{
    public Task SendResponse(SendMessageResponse request,
                             CancellationToken token,
                             int timeoutMs = 10000);
}

public interface IBotticelliBusAgent<in THandler> : IBotticelliBusAgent,
                                                    ISubscriber<THandler, SendMessageRequest, SendMessageResponse>
        where THandler : IHandler<SendMessageRequest, SendMessageResponse>
{
}