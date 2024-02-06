using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Analytics.Shared.Requests;

namespace Botticelli.Server.Analytics.Services;

public interface IMetricsInputService
{
    Task PushMetricAsync(PushMetricRequest<IMetricObject> request, CancellationToken token);
}