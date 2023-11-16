using Botticelli.Analytics.Shared.Requests;
using Botticelli.Analytics.Shared.Responses;
using Botticelli.Server.Analytics.Models;

namespace Botticelli.Server.Analytics.Cache
{
    public interface ICache
    {
        public GetMetricsResponse Get(GetMetricsForIntervalsRequest request);
        public void Set(MetricModel request);
        public GetMetricsIntervalsResponse GetForIntervals(GetMetricsForIntervalsRequest request);
        public void Clear(DateTime until);
    }
}
