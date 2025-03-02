namespace Botticelli.Client.Analytics.Settings;

public class AnalyticsClientSettings
{
    public static string Section => "AnalyticsClient";
    public required string TargetUrl { get; set; }
    public bool UseSsl { get; set; }
}