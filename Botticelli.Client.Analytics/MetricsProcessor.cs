using Botticelli.Analytics.Shared.Metrics;

namespace Botticelli.Client.Analytics;

public class MetricsProcessor
{
    private readonly MetricsPublisher _mediator;

    public MetricsProcessor(MetricsPublisher mediator)
    {
        _mediator = mediator;
    }

    public void Process(object metricObj, string botId)
        => Task.Run(() => _mediator.Publish(new MetricObject
        {
            Name = metricObj.GetType().Name,
            BotId = botId
        }, CancellationToken.None));

    public void Process(string name, string botId)
        => Task.Run(() => _mediator.Publish(new MetricObject
        {
            Name = name,
            Timestamp = DateTime.Now,
            BotId = botId
        }, CancellationToken.None));
}