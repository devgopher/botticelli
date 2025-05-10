using Botticelli.Framework.Events;
using Botticelli.Framework.Extensions.Processors;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Message = Botticelli.Shared.ValueObjects.Message;
using Poll = Botticelli.Shared.ValueObjects.Poll;
using User = Botticelli.Shared.ValueObjects.User;

namespace Botticelli.Framework.Telegram.Handlers;

public class BotUpdateHandler(ILogger<BotUpdateHandler> logger) : IBotUpdateHandler
{
    private readonly MemoryCacheEntryOptions _entryOptions
            = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(24));

    private readonly MemoryCache _memoryCache = new(new MemoryCacheOptions
    {
        ExpirationScanFrequency = TimeSpan.FromMinutes(1)
    });

    private readonly List<IBotUpdateSubHandler> _subHandlers = [];

    public async Task HandleUpdateAsync(ITelegramBotClient botClient,
                                        Update update,
                                        CancellationToken cancellationToken)
    {
        try
        {
            // caching updates in order to avoid message "cloning"
            if (_memoryCache.TryGetValue(update.Id, out _)) return;

            _memoryCache.Set(update.Id, update, _entryOptions);

            logger.LogDebug($"{nameof(HandleUpdateAsync)}() started...");

            var botMessage = update.Message;

            Message? botticelliMessage = null;

            if (botMessage == null)
            {
                if (update.CallbackQuery != null)
                {
                    botMessage = update.CallbackQuery?.Message;

                    if (botMessage == null)
                    {
                        logger.LogError($"{nameof(HandleUpdateAsync)}() {nameof(botMessage)} is null!");

                        return;
                    }

                    botticelliMessage = new Message
                    {
                        ChatIdInnerIdLinks = new Dictionary<string, List<string>>
                        {
                            {
                                update.CallbackQuery?.Message.Chat?.Id.ToString(),
                                [update.CallbackQuery.Message?.MessageId.ToString()]
                            }
                        },
                        ChatIds = [update.CallbackQuery?.Message.Chat.Id.ToString()],
                        CallbackData = update.CallbackQuery?.Data ?? string.Empty,
                        CreatedAt = update.Message?.Date ?? DateTime.Now,
                        LastModifiedAt = update.Message?.Date ?? DateTime.Now,
                        From = new User
                        {
                            Id = update.CallbackQuery?.From.Id.ToString(),
                            Name = update.CallbackQuery?.From.FirstName,
                            Surname = update.CallbackQuery?.From.LastName,
                            Info = string.Empty,
                            IsBot = update.CallbackQuery?.From.IsBot,
                            NickName = update.CallbackQuery?.From.Username
                        }
                    };
                }

                if (update.Poll != null)
                    botticelliMessage = new Message
                    {
                        Subject = string.Empty,
                        Body = string.Empty,
                        Poll = new Poll
                        {
                            Id = update.Poll.Id,
                            IsAnonymous = update.Poll.IsAnonymous,
                            Question = update.Poll.Question,
                            Type = update.Poll.Type.ToLower() == "regular" ? Poll.PollType.Regular : Poll.PollType.Quiz,
                            Variants = update.Poll.Options.Select(o => new ValueTuple<string, int>(o.Text, o.VoterCount)),
                            CorrectAnswerId = update.Poll.CorrectOptionId
                        }
                    };
            }
            else
            {
                botticelliMessage = new Message(botMessage.MessageId.ToString())
                {
                    ChatIdInnerIdLinks = new Dictionary<string, List<string>>
                            {{botMessage.Chat.Id.ToString(), [botMessage.MessageId.ToString()]}},
                    ChatIds = [botMessage.Chat.Id.ToString()],
                    Subject = string.Empty,
                    Body = botMessage.Text ?? string.Empty,
                    LastModifiedAt = botMessage.Date,
                    Attachments = new List<BaseAttachment>(5),
                    CreatedAt = botMessage.Date,
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
                                Latitude = (decimal) botMessage.Location.Latitude,
                                Longitude = (decimal) botMessage.Location.Longitude
                            } :
                            null
                };
            }

            foreach (var subHandler in _subHandlers) await subHandler.Process(botClient, update, cancellationToken);

            if (botticelliMessage != null)
            {
                await Process(botticelliMessage, cancellationToken);

                MessageReceived?.Invoke(this,
                                        new MessageReceivedBotEventArgs
                                        {
                                            Message = botticelliMessage
                                        });
            }

            logger.LogDebug($"{nameof(HandleUpdateAsync)}() finished...");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(HandleUpdateAsync)}() error");
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient,
                                 Exception exception,
                                 HandleErrorSource source,
                                 CancellationToken cancellationToken)
    {
        logger.LogError($"{nameof(HandleErrorAsync)}() error: {exception.Message}", exception);

        return Task.CompletedTask;
    }

    public void AddSubHandler<T>(T subHandler) where T : IBotUpdateSubHandler
    {
        _subHandlers.Add(subHandler);
    }

    public event IBotUpdateHandler.MsgReceivedEventHandler? MessageReceived;

    /// <summary>
    ///     Processes requests
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    protected async Task Process(Message request, CancellationToken token)
    {
        logger.LogDebug($"{nameof(Process)}({request.Uid}) started...");

        if (token is {CanBeCanceled: true, IsCancellationRequested: true}) return;

        var processorFactory = ProcessorFactoryBuilder.Build();

        var clientNonChainedTasks = processorFactory.GetProcessors()
                                                    .Select(p => p.ProcessAsync(request, token));

        var clientChainedTasks = processorFactory.GetCommandChainProcessors()
                                                 .Select(p => p.ProcessAsync(request, token));

        var clientTasks = clientNonChainedTasks.Concat(clientChainedTasks).ToArray();

        await Parallel.ForEachAsync(clientTasks, token, async (t, ct) => await t.WaitAsync(ct));

        logger.LogDebug($"{nameof(Process)}({request.Uid}) finished...");
    }
}