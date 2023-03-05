namespace Botticelli.Bus.Rabbit.Settings;

public class QueueSettings
{
    public string Name { get; set; }
    public bool Durable { get; set; }

    /// <summary>
    /// Tries to create a queue (if false - you should create a queue by yorself)
    /// </summary>
    public bool TryCreate { get; set; }

    /// <summary>
    /// Checks if queue exists on publish. If TryCreate=true - tries to create it,
    /// if false - throws an exception
    /// </summary>
    public bool CheckQueueOnPublish { get; set; }
}