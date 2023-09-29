namespace Botticelli.Analytics.Shared.Metrics;

public class MetricObject
{
    public string Id => Guid.NewGuid().ToString();
    public string BotId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Name { get; set; }
}