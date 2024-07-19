using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using EasyCaching.Core;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

/// <summary>
///     A command with waiting for a response
/// </summary>
/// <typeparam name="TInputCommand"></typeparam>
public abstract class WaitForClientResponseCommandChainProcessor<TInputCommand> : CommandProcessor<TInputCommand>,
    ICommandChainProcessor<TInputCommand>
    where TInputCommand : class, ICommand
{
    private TimeSpan Timeout { get; } = TimeSpan.FromMinutes(1);
    private readonly IEasyCachingProvider _cache;

    protected WaitForClientResponseCommandChainProcessor(ILogger<CommandChainProcessor<TInputCommand>> logger,
        ICommandValidator<TInputCommand> validator,
        MetricsProcessor metricsProcessor,
        IEasyCachingProviderFactory factory) : base(logger, validator, metricsProcessor)
    {
        _cache = factory.GetCachingProvider("botticelli_wait_for_response");
    }

    public virtual void SetBot(IBot bot)
    {
        Bot = bot;

        ((BaseBot)Bot).MessageReceived += async (sender, args) =>
        {
            var chats = args.Message?.ChatIds ?? [..Array.Empty<string>()];

            foreach (var chatId in chats)
            {
                var checkMsg = await _cache.GetAsync<Message>(chatId);
                switch (checkMsg.IsNull)
                {
                    case false when args.Message?.Type == Message.MessageType.Messaging
                                    && checkMsg.Value.LastModifiedAt < args.Message?.LastModifiedAt
                                    && args.Message?.From?.Id != Bot.BotUserId:
                    case true:
                        if (args.Message?.Type == Message.MessageType.Messaging && args.Message?.From?.Id != Bot.BotUserId)
                            await _cache.SetAsync(chatId, args.Message, TimeSpan.FromMinutes(1));
                        break;
                }
            }
        };
    }

    public override async Task ProcessAsync(Message message, CancellationToken token)
    {
        await base.ProcessAsync(message, token);

        if (message.Type != Message.MessageType.Messaging)
            return;

        var started = DateTime.Now;
        Message responseMessage = null;

        // waiting for input
        while (DateTime.Now - started <= Timeout)
        {
            var chatId = message.ChatIds.First()!;
            var checkMsg = await _cache.GetAsync<Message>(chatId, token);

            if (checkMsg.IsNull || (checkMsg.Value.Type != Message.MessageType.Messaging && checkMsg.Value.From?.Id == this.Bot.BotUserId))
                continue;
            
            responseMessage = checkMsg.Value;
            responseMessage.ProcessingArgs ??= new List<string>();
            responseMessage.ProcessingArgs?.Add(checkMsg.Value.Body);

            break;
        }

        if (responseMessage == null)
        {
            _logger.LogInformation("No response from a client for message {uid}", message.Uid);

            return;
        }

        responseMessage.ProcessingArgs?.Add(message.Body);

        if (Next != null)
            await Next.ProcessAsync(responseMessage, token);
        else
            _logger.LogInformation("No Next command for message {uid}", message.Uid);
    }

    public ICommandChainProcessor Next { get; set; }
}