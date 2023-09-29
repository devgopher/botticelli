﻿namespace Botticelli.Server.Analytics.Models
{
    public class MetricModel : IMetricModel<string>
    {
        public string Id { get; set; }
        public string BotId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
