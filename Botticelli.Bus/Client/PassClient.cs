using Botticelli.Bot.Interfaces.Agent;
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

    public async Task<SendMessageResponse> GetResponse(SendMessageRequest request,
                                                       CancellationToken token,
                                                       int timeoutMs = 10000)
    {
       NoneBus.SendMessageRequests.Enqueue(request);

        var waitTask = Task.Run(() =>
                                {
                                    var period = 0;
                                    var delta = 50;

                                    while (period < timeoutMs)
                                    {
                                        if (NoneBus.SendMessageResponses.TryDequeue(out var response))
                                        {
                                            if (response.MessageUid == request.Message.Uid) return response;

                                            NoneBus.SendMessageResponses.Enqueue(response);
                                        }

                                        Task.Delay(delta, token).Wait(token);
                                        period += delta;
                                    }

                                    return new SendMessageResponse(request.Message.Uid, "Timeout")
                                    {
                                        MessageSentStatus = MessageSentStatus.Fail
                                    };
                                },
                                token);

        return waitTask.Result;
    }

    public async Task SendResponse(SendMessageResponse response, CancellationToken tokens) 
        => NoneBus.SendMessageResponses.Enqueue(response);
}