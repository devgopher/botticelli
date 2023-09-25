using Botticelli.Framework.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers;

public class MessageSentHandler : BasicHandler<MessageSentBotEventArgs>
{
    public MessageSentHandler(MetricsPublisher publisher, ILogger<MessageSentBotEventArgs> logger) : base(publisher, logger)
    {
    }
}