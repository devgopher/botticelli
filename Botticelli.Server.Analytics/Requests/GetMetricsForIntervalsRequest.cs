namespace Botticelli.Server.Analytics.Requests;

public class GetMetricsForIntervalsRequest : GetMetricsRequest
{
    public int Interval { get; set; }
}