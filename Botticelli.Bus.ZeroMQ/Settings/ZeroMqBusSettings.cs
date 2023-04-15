namespace Botticelli.Bus.ZeroMQ.Settings;

public class ZeroMqBusSettings : BaseBusSettings
{
    public string TargetUri { get; set; }
    public string ListenUri { get; set; }
}