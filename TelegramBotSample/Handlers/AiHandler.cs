using Botticelli.AI.AIProvider;
using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace TelegramBotSample.Handlers
{
    public class AiHandler : IHandler<SendMessageRequest, SendMessageResponse>
    {
        private readonly IAiProvider _provider;

        public AiHandler(IAiProvider provider) => _provider = provider;

        public async Task<SendMessageResponse> Handle(SendMessageRequest input, CancellationToken token)
        {
            try
            {
                await _provider.SendAsync(new AiMessage(input.Uid)
                                          {
                                              ChatId = input.Message.ChatId,
                                              Body = input.Message.Body,
                                              Subject = input.Message.Subject
                                          },
                                          token);

                return new SendMessageResponse(input.Uid, string.Empty)
                {
                    MessageUid = input.Uid,
                    MessageSentStatus = MessageSentStatus.Ok
                };
            }
            catch (Exception ex)
            {
                return new SendMessageResponse(input.Uid, ex.Message)
                {
                    MessageUid = input.Uid,
                    MessageSentStatus = MessageSentStatus.Fail
                };
            }
        }
    }
}
