using Botticelli.Client.Analytics.Requests;
using MediatR;

namespace Botticelli.Client.Analytics;

public class MetricsProcessor
{
    private readonly IMediator _mediator;

    public MetricsProcessor(IMediator mediator) => _mediator = mediator;

    public void Process(object metricObj)
        => Task.Run(() => _mediator.Publish(new MetricRequest
        {
            MetricName = metricObj.GetType().Name
        }));

    public void Process(string name)
        => Task.Run(() => _mediator.Publish(new MetricRequest
        {
            MetricName = name
        }));
}