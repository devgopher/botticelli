namespace Botticelli.Analytics.Shared.Metrics;

public class MetricObject : IMetricObject
{
    public string Id => Guid.NewGuid().ToString();
    public string BotId { get; set; }
    public DateTime Timestamp { get; set; }
    public virtual string Name { get; set; }
    public Dictionary<string, string> AdditionalParameters { get; set; }
}