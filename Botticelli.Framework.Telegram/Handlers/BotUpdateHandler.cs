using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.MessageProcessors;
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

            if (botMessage == null) return;

            var botticelliMessage = new Message(botMessage.MessageId.ToString())
            {
                ChatIdInnerIdLinks = new Dictionary<string, List<string>>
                    { { botMessage.Chat.Id.ToString(), new List<string> { botMessage.MessageId.ToString() } } },
                ChatIds = new List<string> { botMessage.Chat.Id.ToString() },
                Subject = string.Empty,
                Body = botMessage.Text,
                Attachments = new List<IAttachment>(5),
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
                Location = botMessage.Location != null
                    ? new GeoLocation
                    {
                        Latitude = (decimal)botMessage.Location?.Latitude,
                        Longitude = (decimal)botMessage.Location?.Longitude
                    }
                    : null
            };

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
            .Where(p => p.GetType() != typeof(ChatMessageProcessor))
            .Select(async p => await p.ProcessAsync(request, token));

        
        Parallel.ForEach(clientTasks, async task => await task.WaitAsync(token));
        _logger.LogDebug($"{nameof(Process)}({request.Uid}) finished...");

        return Task.CompletedTask;
    }
}