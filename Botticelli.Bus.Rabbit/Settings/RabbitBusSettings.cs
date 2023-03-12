namespace Botticelli.Bus.Rabbit.Settings;

public class RabbitBusSettings : BaseBusSettings
{
    public string VHost { get; set; }
    public string Uri { get; set; }
    public string Exchange { get; set; }
    public QueueSettings QueueSettings { get; set; }
}