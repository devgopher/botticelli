using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace Botticelli.Bus.None.Handler
{
    public class DefaultPassHandler : IHandler<SendMessageRequest, SendMessageResponse>
    {
        public async Task<SendMessageResponse> Handle(SendMessageRequest input, CancellationToken token)
            => new(input.Uid, string.Empty)
            {
                MessageSentStatus = MessageSentStatus.Ok,
                MessageUid = input.Uid
            };
    }
}
