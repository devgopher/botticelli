namespace Botticelli.Server.Analytics.Models
{
    public class CountModel : IMetricModel<decimal>
    {
        public string Id { get; set; }
        public string BotId { get; set; }
        public DateTime Timestamp { get; set; }

        public decimal Value { get; set; }
    }
}
