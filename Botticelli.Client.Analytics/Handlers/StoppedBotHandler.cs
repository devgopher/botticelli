using Botticelli.Framework.Events;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers;

public class StoppedBotHandler : BasicHandler<StoppedBotEventArgs>
{
    public StoppedBotHandler(MetricsPublisher publisher, ILogger<StoppedBotEventArgs> logger) : base(publisher, logger)
    {
    }
}