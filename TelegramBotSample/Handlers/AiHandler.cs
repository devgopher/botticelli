using Botticelli.AI.AIProvider;
using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;

namespace TelegramBotSample.Handlers;

public class AiHandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    private readonly IAiProvider _provider;
    private readonly ILogger<AiHandler> _logger;

    public AiHandler(IAiProvider provider, ILogger<AiHandler> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    ///<inheritdoc/>
    public async Task Handle(SendMessageRequest input, CancellationToken token)
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
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while handling a message from AI backend: {ex.Message}", ex);
        } 
    }
}