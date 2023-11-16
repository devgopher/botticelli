using Botticelli.Analytics.Shared.Requests;
using Botticelli.Analytics.Shared.Responses;

namespace Botticelli.Server.Analytics.Cache
{
    public interface ICache
    {
        public GetMetricsResponse Get(GetMetricsForIntervalsRequest request);
        public void SetAsync(PushMetricRequest request);
        public GetMetricsIntervalsResponse GetAsync(GetMetricsForIntervalsRequest request);
        public void Clear(DateTime until);
    }
}
