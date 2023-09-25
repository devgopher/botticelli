using Botticelli.Framework.Events;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers;


public class StartedBotHandler : BasicHandler<StartedBotEventArgs>
{
    public StartedBotHandler(MetricsPublisher publisher, ILogger<StartedBotEventArgs> logger) : base(publisher, logger)
    {
    }
}