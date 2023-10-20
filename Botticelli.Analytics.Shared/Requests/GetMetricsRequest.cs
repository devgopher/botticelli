namespace Botticelli.Analytics.Shared.Requests
{
    public class GetMetricsRequest
    {
        public string BotId { get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
