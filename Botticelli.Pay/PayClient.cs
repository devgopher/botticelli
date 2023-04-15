using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Pay;

public class PayClient : IBotticelliBusAgent
{
    public async Task SendResponseAsync(SendMessageResponse response,
                                   CancellationToken token,
                                   int timeoutMs = 10000) => throw new NotImplementedException();

    public Task StartAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task StopAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

    public void Dispose()
    {
    }
}