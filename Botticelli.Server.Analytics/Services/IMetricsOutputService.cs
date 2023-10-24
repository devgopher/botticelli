using Botticelli.Analytics.Shared.Requests;
using Botticelli.Analytics.Shared.Responses;

namespace Botticelli.Server.Analytics.Services;

public interface IMetricsOutputService
{
    Task<GetMetricsResponse> GetMetricsAsync(GetMetricsRequest request,
        CancellationToken token);

    Task<GetMetricsIntervalsResponse> GetMetricsForIntervalAsync(GetMetricsForIntervalsRequest request,
        CancellationToken token);

    Task<IEnumerable<string>> GetMetricNamesAsync(CancellationToken token);
}