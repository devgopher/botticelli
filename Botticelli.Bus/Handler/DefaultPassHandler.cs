using Botticelli.Bot.Interfaces.Bus.Handlers;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bus.None.Handler;

public class DefaultPassHandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    public async Task Handle(SendMessageRequest input, CancellationToken token)
    {
    }
}