using Botticelli.Bot.Interfaces.Bus.Handlers;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.ValueObjects;

namespace TelegramInlineLayoutsSample.Handlers;

public class GetCalendarHandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    private readonly IBot _bot;
    private readonly ILogger<GetCalendarHandler> _logger;

    public GetCalendarHandler(IBot bot,  ILogger<GetCalendarHandler> logger)
    {
        _bot = bot;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(SendMessageRequest input, CancellationToken token)
    {
        try
        {
            var message = 
            await _bot.SendMessageAsync();

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while handling a message from AI backend: {ex.Message}", ex);
        }
    }
}