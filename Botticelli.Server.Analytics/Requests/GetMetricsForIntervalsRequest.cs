namespace Botticelli.Server.Analytics.Requests;

public class GetMetricsForIntervalsRequest : GetMetricsRequest
{
    public TimeSpan Interval { get; set; }
}