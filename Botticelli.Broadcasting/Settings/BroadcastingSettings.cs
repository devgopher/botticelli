namespace Botticelli.Broadcasting.Settings;

public class BroadcastingSettings
{
    public const string Section = "Broadcasting";
    public required string BotId { get; set; }
    public TimeSpan? HowOld { get; set; } = TimeSpan.FromSeconds(60);
    public required string ServerUri { get; set; }
    public required string ConnectionString { get; set; }
}