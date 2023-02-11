using Botticelli.Bot.Interfaces.Client;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bus.Client;

public class PassClient<TBot> : IBotticelliBusClient
        where TBot : IBot
{
    private readonly TBot _bot;

    public PassClient(TBot bot) => _bot = bot;

    public async Task<SendMessageResponse> GetResponse(SendMessageRequest request,
                                                       CancellationToken token,
                                                       int timeoutMs = 10000)
        => await _bot.SendMessageAsync(request, token);
}