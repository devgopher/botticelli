using Botticelli.AI.AIProvider;
using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace TelegramBotSample.Handlers;

public class AiHandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    private readonly IAiProvider _provider;

    public AiHandler(IAiProvider provider) => _provider = provider;

    public async Task Handle(SendMessageRequest input, CancellationToken token)
    {
            await _provider.SendAsync(new AiMessage(input.Uid)
                                      {
                                          ChatId = input.Message.ChatId,
                                          Body = input.Message.Body,
                                          Subject = input.Message.Subject
                                      },
                                      token);
    }
}