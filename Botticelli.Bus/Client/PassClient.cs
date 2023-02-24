using Botticelli.Bot.Interfaces.Client;
using Botticelli.Bus.None.Bus;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bus.None.Client;

public class PassClient<TBot> : IBotticelliBusClient
        where TBot : IBot
{
    private readonly TBot _bot;

    public PassClient(TBot bot) => _bot = bot;

    public async Task<SendMessageResponse> GetResponse(SendMessageRequest request,
                                                       CancellationToken token,
                                                       int timeoutMs = 10000)
    {
        await _bot.SendMessageAsync(request, token);

        var waitTask = Task.Run(() =>
        {
            int period = 0;
            int delta = 50;

            while (period < timeoutMs)
            {
                if (NoneBus.SendMessageResponses.TryDequeue(out var response))
                {
                    if (response.MessageUid == request.Message.Uid)
                        return response;
                    NoneBus.SendMessageResponses.Enqueue(response);
                }

                Task.Delay(delta, token).Wait(token);
                period += delta;
            }

            return new SendMessageResponse(request.Message.Uid, $"Timeout")
            {
                MessageSentStatus = MessageSentStatus.Fail
            };
        }, token);

        return waitTask.Result;
    }
}