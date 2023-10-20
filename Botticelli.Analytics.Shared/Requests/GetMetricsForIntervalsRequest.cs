namespace Botticelli.Analytics.Shared.Requests;

public class GetMetricsForIntervalsRequest : GetMetricsRequest
{
    public int Interval { get; set; }
}