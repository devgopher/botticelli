using Botticelli.Server.Analytics.Requests;
using Botticelli.Server.Analytics.Responses;

namespace Botticelli.Server.Analytics.Services;

public interface IMetricsOutputService
{
    Task<GetMetricsResponse> GetMetricsAsync(GetMetricsRequest request,
        CancellationToken token);

    Task<GetMetricsIntervalsResponse> GetMetricsForIntervalAsync(GetMetricsForIntervalsRequest request,
        CancellationToken token);
}