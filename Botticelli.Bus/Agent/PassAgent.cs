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
public class PassAgent<THandler> : IBotticelliBusAgent<THandler>
        where THandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    private readonly THandler _handler;

    public PassAgent(THandler handler) => _handler = handler;

    /// <summary>
    ///     Sends a response
    /// </summary>
    /// <param name="response"></param>
    /// <param name="token"></param>
    /// <param name="timeoutMs"></param>
    /// <returns></returns>
    public async Task SendResponseAsync(SendMessageResponse response,
                                   CancellationToken token,
                                   int timeoutMs = 10000) =>
            NoneBus.SendMessageResponses.Enqueue(response);

    public async Task StartAsync(CancellationToken token)
    {
        await Task.Run(async () => await InnerProcess(_handler, token));
    }

    public Task StopAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

    private async Task InnerProcess(THandler handler, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (NoneBus.SendMessageRequests.TryDequeue(out var request)) await handler.Handle(request, token);
            Thread.Sleep(5);
        }
    }
}