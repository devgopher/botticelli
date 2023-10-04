using Botticelli.Server.Analytics.Requests;

namespace Botticelli.Server.Analytics.Services;

public interface IMetricsInputService
{
    Task PushMetricAsync(PushMetricRequest request, CancellationToken token);
}