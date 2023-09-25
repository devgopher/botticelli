using Botticelli.Framework.Events;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers;

public class MessageRemovedHandler : BasicHandler<MessageRemovedBotEventArgs>
{
    public MessageRemovedHandler(MetricsPublisher publisher, ILogger<MessageRemovedBotEventArgs> logger) : base(publisher, logger)
    {
    }
}