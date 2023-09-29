namespace Botticelli.Server.Analytics.Requests
{
    public class GetMetricsRequest
    {
        public string BotId { get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class GetMetricsForIntervalsRequest : GetMetricsRequest
    {
        public TimeSpan Interval { get; set; }
    }
}
