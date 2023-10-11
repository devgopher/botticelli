using Botticelli.Client.Analytics.Requests;
using MediatR;

namespace Botticelli.Client.Analytics;

public class MetricsProcessor
{
    private readonly IMediator _mediator;

    public MetricsProcessor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Process(object metricObj, CancellationToken token)
        => await _mediator.Publish(new MetricRequest
        {
            MetricName = metricObj.GetType().Name
        }, token);
}