using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bus.ZeroMQTests.Handler
{
    public class TestHandler : IHandler<SendMessageRequest, SendMessageResponse>
    {
        public async Task Handle(SendMessageRequest input, CancellationToken token)
        {

        }
    }
}
