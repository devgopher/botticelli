using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Bus.None.Bus;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Bus.None.Agent;

/// <summary>
///     Simple pass agent (no bus)
/// </summary>
/// <typeparam name="THandler"></typeparam>
public class PassAgent<THandler> : IBotticelliBusAgent<THandler>
        where THandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    //private readonly IList<THandler> _handlers = new List<THandler>(5);
    private readonly IServiceProvider _sp;
    private readonly IServiceScope _scope;

    public PassAgent(IServiceProvider sp)
    {
        _sp = sp;
        _scope = _sp.CreateScope();
    }

    /// <summary>
    ///     Sends a response
    /// </summary>
    /// <param name="response"></param>
    /// <param name="token"></param>
    /// <param name="timeoutMs"></param>
    /// <returns></returns>
    public async Task SendResponse(SendMessageResponse response,
                                   CancellationToken token,
                                   int timeoutMs = 10000) =>
            NoneBus.SendMessageResponses.Enqueue(response);

    private async Task InnerProcess(THandler handler, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (NoneBus.SendMessageRequests.TryDequeue(out var request)) NoneBus.SendMessageResponses.Enqueue(await handler.Handle(request, CancellationToken.None));
            Thread.Sleep(5);
        }
    }

    public async Task StartAsync(CancellationToken token)
    {
        var handler = _scope.ServiceProvider.GetRequiredService<THandler>();
        Task.Run(async () => await InnerProcess(handler, token));
    }

    public Task StopAsync(CancellationToken cancellationToken) => throw new NotImplementedException();
}