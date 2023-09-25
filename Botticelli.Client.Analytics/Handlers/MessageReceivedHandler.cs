using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Framework.Events;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers
{
    public class MessageReceivedHandler : BasicHandler<MessageReceivedBotEventArgs, MessageRemovedMetric>
    {
        public MessageReceivedHandler(BotContext context, MetricsPublisher publisher, 
            ILogger<MessageReceivedBotEventArgs> logger) : base(context, publisher, logger)
        {
        }

        protected override MessageRemovedMetric Convert(MessageReceivedBotEventArgs args, string botId) => throw new NotImplementedException();
    }
}
