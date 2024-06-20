using Botticelli.Framework.Commands.Processors;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Message = Botticelli.Shared.ValueObjects.Message;
using User = Botticelli.Shared.ValueObjects.User;

namespace Botticelli.Framework.Telegram.Handlers;

public class BotUpdateHandler : IBotUpdateHandler
{
    private readonly ILogger<BotUpdateHandler> _logger;
    private readonly ClientProcessorFactory _processorFactory;

    public BotUpdateHandler(ILogger<BotUpdateHandler> logger, ClientProcessorFactory processorFactory)
    {
        _logger = logger;
        _processorFactory = processorFactory;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError($"{nameof(HandlePollingErrorAsync)}() error: {exception.Message}", exception);
        Thread.Sleep(500);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug($"{nameof(HandleUpdateAsync)}() started...");

            var botMessage = update.Message;
            Message botticelliMessage;

            if (botMessage == null)
            {
                if (update.CallbackQuery != null)
                    botticelliMessage = new Message()
                    {
                        ChatIdInnerIdLinks = new Dictionary<string, List<string>>
                                {{update.CallbackQuery?.Message.Chat?.Id.ToString(), [update.CallbackQuery.Message?.MessageId.ToString()]}},
                        ChatIds = [update.CallbackQuery?.Message.Chat?.Id.ToString()],
                        CallbackData = update.CallbackQuery?.Data ?? string.Empty
                    };
                else
                    return;
            }
            else
            {
                botticelliMessage = new Message(botMessage.MessageId.ToString())
                {
                    ChatIdInnerIdLinks = new Dictionary<string, List<string>>
                            {{botMessage.Chat.Id.ToString(), [botMessage.MessageId.ToString()]}},
                    ChatIds = [botMessage.Chat.Id.ToString()],
                    Subject = string.Empty,
                    Body = botMessage?.Text ?? string.Empty,
                    LastModifiedAt = botMessage.Date,
                    Attachments = new List<BaseAttachment>(5),
                    From = new User
                    {
                        Id = botMessage.From?.Id.ToString(),
                        Name = botMessage.From?.FirstName,
                        Surname = botMessage.From?.LastName,
                        Info = string.Empty,
                        IsBot = botMessage.From?.IsBot,
                        NickName = botMessage.From?.Username
                    },
                    ForwardedFrom = new User
                    {
                        Id = botMessage.ForwardFrom?.Id.ToString(),
                        Name = botMessage.ForwardFrom?.FirstName,
                        Surname = botMessage.ForwardFrom?.LastName,
                        Info = string.Empty,
                        IsBot = botMessage.ForwardFrom?.IsBot,
                        NickName = botMessage.ForwardFrom?.Username
                    },
                    Location = botMessage.Location != null ?
                            new GeoLocation
                            {
                                Latitude = (decimal) botMessage.Location?.Latitude,
                                Longitude = (decimal) botMessage.Location?.Longitude
                            } :
                            null
                };
            }

            await Process(botticelliMessage, cancellationToken);

            _logger.LogDebug($"{nameof(HandleUpdateAsync)}() finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(HandleUpdateAsync)}() error");
        }
    }

    public event IBotUpdateHandler.MsgReceivedEventHandler? MessageReceived;

    /// <summary>
    ///     Processes requests
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    protected Task Process(Message request, CancellationToken token)
    {
        _logger.LogDebug($"{nameof(Process)}({request.Uid}) started...");

        if (token is { CanBeCanceled: true, IsCancellationRequested: true })
            return Task.CompletedTask;
        
        var clientTasks = _processorFactory
            .GetProcessors()
            .Select(p => p.ProcessAsync(request, token));

        Parallel.ForEachAsync(clientTasks, token, async (t, ct) => await t.WaitAsync(ct));
        
        _logger.LogDebug($"{nameof(Process)}({request.Uid}) finished...");
        return Task.CompletedTask;
    }
}