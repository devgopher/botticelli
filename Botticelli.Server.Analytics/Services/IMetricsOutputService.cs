using Botticelli.Server.Analytics.Requests;
using Botticelli.Server.Analytics.Responses;

namespace Botticelli.Server.Analytics.Services;

public interface IMetricsOutputService
{
    Task PushMetric(PushMetricRequest request, CancellationToken token);
    GetMetricsResponse GetMetrics(GetMetricsRequest request);
    GetMetricsIntervalsResponse GetMetricsForInterval(GetMetricsForIntervalsRequest request);
}