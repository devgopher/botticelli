namespace Botticelli.Framework.Options;

/// <summary>
///     Bot data/context settings
/// </summary>
public class BotDataSettings
{
    public const string Section = "BotData";

    public string? BotId { get; set; }
    public string? BotKey { get; set; }
}