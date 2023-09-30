using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Shared.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers;

public abstract class BasicHandler<TArgs, TMetric> : IRequestHandler<TArgs> where TArgs : IRequest
{
    private readonly BotContext _context;
    private readonly MetricsPublisher _publisher;
    private readonly ILogger _logger;

    protected BasicHandler(BotContext context, MetricsPublisher publisher, ILogger logger)
    {
        _context = context;
        _publisher = publisher;
        _logger = logger;
    }
    
    public virtual async Task Handle(TArgs request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogTrace($"Metric {request.GetType().Name} handling...");

            var metric = Convert(request, _context.BotId);

            await _publisher.Publish(metric, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }
    }

    protected virtual MetricObject Convert(TArgs args, string botId) =>
            new()
            {
                BotId = botId,
                Name = typeof(TArgs).Name,
                Timestamp = DateTime.Now
            };
}