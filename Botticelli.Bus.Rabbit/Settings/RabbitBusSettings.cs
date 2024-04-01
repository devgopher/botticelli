namespace Botticelli.Bus.Rabbit.Settings;

public class RabbitBusSettings : BaseBusSettings
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string VHost { get; set; }
    public string Uri { get; set; }
    public string Exchange { get; set; }

    public string ExchangeType { get; set; } = "direct";
    public QueueSettings QueueSettings { get; set; }
}