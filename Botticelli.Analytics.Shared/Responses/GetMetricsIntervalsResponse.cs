namespace Botticelli.Analytics.Shared.Responses
{
    public class GetMetricsIntervalsResponse : GetMetricsResponse
    {
        public IEnumerable<GetMetricsResponse> MetricsForIntervals { get; set; }
    }
}
