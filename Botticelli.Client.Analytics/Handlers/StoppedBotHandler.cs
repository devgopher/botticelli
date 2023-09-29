using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Framework.Events;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers;

public class StoppedBotHandler : BasicHandler<StoppedBotEventArgs, MetricObject>
{
    public StoppedBotHandler(BotContext context, MetricsPublisher publisher, ILogger<StoppedBotEventArgs> logger) : base(context, publisher, logger)
    {
    }

    protected override MetricObject Convert(StoppedBotEventArgs args, string botId) => new()
    {
        BotId = botId
    };
}