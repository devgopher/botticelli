namespace Botticelli.Server.Analytics.Models;

public interface IMetricModel<T>
{
    string Id { get; set; }
    string BotId { get; set; }
    DateTime Timestamp { get; set; }
    public T Value { get; set; }
}