using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Shared.API.Client.Requests;

namespace Botticelli.Pay;

public class PayClient : IBotticelliBusAgent
{
    public async Task SendResponse(SendMessageRequest request,
                                   CancellationToken token,
                                   int timeoutMs = 10000) => throw new NotImplementedException();
}