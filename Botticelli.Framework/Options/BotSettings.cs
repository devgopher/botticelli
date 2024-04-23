using Botticelli.SecureStorage.Settings;

namespace Botticelli.Framework.Options;

/// <summary>
///     Bot general settings
/// </summary>
public abstract class BotSettings
{
    /// <summary>
    ///     Bot name
    /// </summary>
    public string Name { get; set; }
    public SecureStorageSettings SecureStorageSettings { get; set; }

    public string BotCertificateName { get; set; } = "BotticelliBotsBot";

    public LogTargetSettings LogTargetSettings { get; set; }
}