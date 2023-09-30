using System.ComponentModel.DataAnnotations;

namespace Botticelli.Server.Analytics.Models
{
    public class MetricModel : IMetricModel
    {
        [Key]
        public string Id { get; set; }
        public string BotId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Name { get; set; }
        public string InternalValue { get; set; }
    }
}
