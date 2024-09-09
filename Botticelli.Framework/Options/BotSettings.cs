namespace Botticelli.Framework.Options;

/// <summary>
///     Bot general settings
/// </summary>
public abstract class BotSettings
{
    /// <summary>
    ///     Bot name
    /// </summary>
    public string? Name { get; set; }


    /// <summary>
    ///     Bot secure storage connection
    /// </summary>
    public string? SecureStorageConnectionString { get; set; }

    public string BotCertificateName { get; set; } = "BotticelliBotsBot";
}