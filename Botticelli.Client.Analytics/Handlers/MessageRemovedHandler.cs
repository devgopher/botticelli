using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Framework.Events;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers;

public class MessageRemovedHandler : BasicHandler<MessageRemovedBotEventArgs, MessageRemovedMetric>
{
    public MessageRemovedHandler(BotContext context, MetricsPublisher publisher, ILogger<MessageRemovedBotEventArgs> logger) : base(context, publisher, logger)
    {
    }

    protected override MessageRemovedMetric Convert(MessageRemovedBotEventArgs args, string botId)
        => new()
        {
            BotId = botId,
            MessageId = args.MessageUid
        };
}