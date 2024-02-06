using Botticelli.Analytics.Shared.Metrics;

namespace Botticelli.Analytics.Shared.Requests;

public class PushMetricRequest<TMetricObject>
where TMetricObject : IMetricObject
{
    public TMetricObject Object { get; set; }
}