using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Shared.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers;

public abstract class BasicHandler<TArgs, TMetric> : IRequestHandler<TArgs> where TArgs : IRequest
{
    private readonly BotContext _context;
    private readonly MetricsPublisher _metricsPublisher;
    private readonly ILogger _logger;

    protected BasicHandler(BotContext context, MetricsPublisher metricsPublisher, ILogger logger)
    {
        _context = context;
        _metricsPublisher = metricsPublisher;
        _logger = logger;
    }
    
    public virtual async Task Handle(TArgs request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogTrace($"Metric {request.GetType().Name} handling...");

            var metric = Convert(request, _context.BotId);

            await _metricsPublisher.Publish(metric, cancellationToken);
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