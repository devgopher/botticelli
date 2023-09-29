using Botticelli.Server.Analytics.Requests;
using Botticelli.Server.Analytics.Responses;

namespace Botticelli.Server.Analytics.Services;

public class MetricsOutputService : IMetricsOutputService
{
    private readonly MetricsReaderWriter _rw;

    public MetricsOutputService(MetricsReaderWriter rw) => _rw = rw;

    public async Task PushMetric(PushMetricRequest request, CancellationToken token)
    {
        await _rw.WriteAsync(request, token);
    }

    public GetMetricsResponse GetMetrics(GetMetricsRequest request)
    {
        var count = _rw.ReadCount(x =>
                                          x.Timestamp >= request.From && x.Timestamp <= request.To && x.BotId == request.BotId && x.Name == request.Name);

        return new GetMetricsResponse
        {
            From = request.From,
            To = request.To,
            Count = count
        };
    }

    public GetMetricsIntervalsResponse GetMetricsForInterval(GetMetricsForIntervalsRequest request)
    {
        var metrics = _rw.ReadCountForFrames(request.Name,
                                             request.BotId,
                                             request.From,
                                             request.To,
                                             request.Interval);
        var metricsForIntervals = metrics.Select(m => new GetMetricsResponse
                                         {
                                             Count = m.count,
                                             From = m.dt1,
                                             To = m.dt2
                                         })
                                         .ToList();

        var commonCount = metricsForIntervals.Sum(m => m.Count);

        return new GetMetricsIntervalsResponse
        {
            From = request.From,
            To = request.To,
            Count = commonCount,
            MetricsForIntervals = metricsForIntervals
        };
    }
}