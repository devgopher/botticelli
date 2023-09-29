namespace Botticelli.Server.Analytics.Responses
{
    public class GetMetricsIntervalsResponse : GetMetricsResponse
    {
        public IEnumerable<GetMetricsResponse> MetricsForIntervals { get; set; }
    }
}
