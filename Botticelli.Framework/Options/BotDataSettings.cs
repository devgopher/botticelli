namespace Botticelli.Framework.Options;

/// <summary>
///     Bot data/context settings
/// </summary>
public abstract class BotDataSettings
{
    public required string BotId { get; set; }
    public string? BotKey { get; set; }
}