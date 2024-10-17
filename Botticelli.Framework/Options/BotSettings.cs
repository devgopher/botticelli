namespace Botticelli.Framework.Options;

/// <summary>
///     Bot general settings
/// </summary>
public abstract class BotSettings : IBotSettings
{
    public abstract string Section { get; }

    /// <summary>
    ///     Bot name
    /// </summary>
    public string? Name { get; set; }
    
    public string BotCertificateName { get; set; } = "BotticelliBotsBot";
}