using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Hosting;

namespace Botticelli.Bot.Interfaces.Agent;

/// <summary>
///     Bus agent works on the side of a endpoint
/// </summary>
public interface IBotticelliBusAgent : IHostedService
{
    public Task SendResponse(SendMessageResponse response,
                             CancellationToken token,
                             int timeoutMs = 10000);
}

public interface IBotticelliBusAgent<in THandler> : IBotticelliBusAgent
        where THandler : IHandler<SendMessageRequest, SendMessageResponse>
{
}