using Botticelli.Framework.Events;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers
{
    public class MessageReceivedHandler : BasicHandler<MessageReceivedBotEventArgs>
    {
        public MessageReceivedHandler(MetricsPublisher publisher, ILogger<MessageReceivedBotEventArgs> logger) : base(publisher, logger)
        {
        }
    }
}
