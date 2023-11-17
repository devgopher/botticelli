using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Client.Analytics.Requests;
using Botticelli.Shared.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers;

public abstract class MetricsHandler<TMetric> : IRequestHandler<IMetricRequest>
{
    private readonly BotContext _context;
    private readonly ILogger _logger;
    private readonly MetricsPublisher _metricsPublisher;

    protected MetricsHandler(BotContext context, MetricsPublisher metricsPublisher, ILogger logger)
    {
        _context = context;
        _metricsPublisher = metricsPublisher;
        _logger = logger;
    }

    public virtual async Task Handle(IMetricRequest request, CancellationToken cancellationToken)
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

    protected virtual MetricObject Convert(IMetricRequest args, string botId) =>
        new()
        {
            BotId = botId,
            Name = args.MetricName,
            Timestamp = DateTime.Now
        };
}