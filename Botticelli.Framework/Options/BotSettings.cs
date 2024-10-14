namespace Botticelli.Framework.Options;

/// <summary>
///     Bot general settings
/// </summary>
public abstract class BotSettings
{
    public static string Section { get; protected set; }

    /// <summary>
    ///     Bot name
    /// </summary>
    public string? Name { get; set; }
    
    public string BotCertificateName { get; set; } = "BotticelliBotsBot";
}