using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Bus.None.Bus;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bus.None.Agent;

/// <summary>
///     Simple pass agent (no bus)
/// </summary>
/// <typeparam name="THandler"></typeparam>
public class PassAgent<THandler> : IBotticelliBusAgent<THandler> where THandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    private readonly IList<THandler> _handlers = new List<THandler>(5);
    private readonly bool isStarted = false;

    /// <summary>
    /// Sends a response
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <param name="timeoutMs"></param>
    /// <returns></returns>
    public async Task SendResponse(SendMessageRequest request,
                                   CancellationToken token,
                                   int timeoutMs = 10000) =>
            NoneBus.SendMessageResponses.Enqueue(request);

    /// <summary>
    /// Subscription
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task Subscribe(THandler handler, CancellationToken token)
    {
        _handlers.Add(handler);

        if (!isStarted) await InnerProcess(handler, token);
    }

    private async Task InnerProcess(THandler handler, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (NoneBus.SendMessageRequests.TryDequeue(out var request)) await handler.Handle(request);
            Thread.Sleep(5);
        }
    }
}