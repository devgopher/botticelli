using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Framework.Events;
using Botticelli.Shared.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers;

public class MessageSentHandler : BasicHandler<MessageSentBotEventArgs, MessageSentMetric>
{
    public MessageSentHandler(BotContext context, MetricsPublisher publisher, ILogger<MessageSentBotEventArgs> logger) : base(context, publisher, logger)
    {
    }

    protected override MessageSentMetric Convert(MessageSentBotEventArgs args, string botId) => throw new NotImplementedException();
}