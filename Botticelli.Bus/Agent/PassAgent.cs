using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Bus.Handlers;
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

    public PassAgent(THandler handler)
    {
        _handler = handler;
    }

    public Task Subscribe(CancellationToken token)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Sends a response
    /// </summary>
    /// <param name="response"></param>
    /// <param name="token"></param>
    /// <param name="timeoutMs"></param>
    /// <returns></returns>
    public Task SendResponseAsync(SendMessageResponse response,
                                  CancellationToken token,
                                  int timeoutMs = 10000)
    {
        NoneBus.SendMessageResponses.Enqueue(response);

        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken token)
    {
        Task.Run(() => InnerProcess(_handler, token), token);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private void InnerProcess(THandler handler, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (NoneBus.SendMessageRequests.TryDequeue(out var request)) 
                handler.Handle(request, token).Wait(token);
            
            Task.Delay(5, token).Wait(token);
        }
    }
}