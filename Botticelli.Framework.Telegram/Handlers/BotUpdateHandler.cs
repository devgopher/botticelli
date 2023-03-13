using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.MessageProcessors;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using Polly;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Message = Botticelli.Shared.ValueObjects.Message;
using User = Botticelli.Shared.ValueObjects.User;

namespace Botticelli.Framework.Telegram.Handlers;

public class BotUpdateHandler : IUpdateHandler
{
    private readonly ILogger<BotUpdateHandler> _logger;
    private readonly ClientProcessorFactory _processorFactory;

    public BotUpdateHandler(ILogger<BotUpdateHandler> logger, ClientProcessorFactory processorFactory)
    {
        _logger = logger;
        _processorFactory = processorFactory;
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient,
                                              Exception exception,
                                              CancellationToken cancellationToken) =>
            _logger.LogError($"{nameof(HandlePollingErrorAsync)}() error: {exception.Message}", exception);

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
                ChatId = botMessage.Chat.Id.ToString(),
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
                ForwardFrom = new User
                {
                    Id = botMessage.ForwardFrom?.Id.ToString(),
                    Name = botMessage.ForwardFrom?.FirstName,
                    Surname = botMessage.ForwardFrom?.LastName,
                    Info = string.Empty,
                    IsBot = botMessage.ForwardFrom?.IsBot,
                    NickName = botMessage.ForwardFrom?.Username
                }
            };

            await Process(botticelliMessage, cancellationToken);

            _logger.LogDebug($"{nameof(HandleUpdateAsync)}() finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(HandleUpdateAsync)}() error", ex);
        }
    }

    /// <summary>
    ///     Processes requests
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    protected async Task Process(Message request, CancellationToken token)
    {
        _logger.LogDebug($"{nameof(Process)}({request.Uid}) started...");

        if (token is {CanBeCanceled: true, IsCancellationRequested: true}) return;

        var clientTasks = _processorFactory
                          .GetProcessors()
                          .Where(p => p.GetType() != typeof(ChatMessageProcessor))
                          .Select(p => p.ProcessAsync(request, token));


        Task.WaitAll(clientTasks.ToArray());

        _logger.LogDebug($"{nameof(Process)}({request.Uid}) finished...");
    }
}