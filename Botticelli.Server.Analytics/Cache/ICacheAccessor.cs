using Botticelli.Analytics.Shared.Requests;
using Botticelli.Analytics.Shared.Responses;
using Botticelli.Server.Analytics.Models;

namespace Botticelli.Server.Analytics.Cache;

public interface ICacheAccessor
{
    public void Set(MetricModel request);
    public int ReadCount(Func<MetricModel, bool> func);
    public GetMetricsIntervalsResponse GetForIntervals(GetMetricsForIntervalsRequest request);
    public void Clear(DateTime until);
    public void Remove(MetricModel request);
}