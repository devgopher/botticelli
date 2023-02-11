using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bus.Agent
{
    /// <summary>
    /// Simple pass agent (no bus)
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    public class PassAgent<TBot> : IBotticelliBusAgent
            where TBot : IBot
    {
        private readonly TBot _bot;

        public PassAgent(TBot bot) => _bot = bot;

        public async Task<SendMessageResponse> SendResponse(SendMessageRequest request, 
                                                            CancellationToken token,
                                                            int timeoutMs = 10000) =>
                await _bot.SendMessageAsync(request, token);
    }
}
