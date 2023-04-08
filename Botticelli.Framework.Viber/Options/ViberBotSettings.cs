using Botticelli.Framework.Options;

namespace Botticelli.Framework.Viber.Options;

/// <inheritdoc />
public class ViberBotSettings : BotSettings
{
    /// <summary>
    ///     Viber API token
    /// </summary>
    public string ViberToken { get; set; } = "50bd0b7b48a7dd24-4888ec418a215884-e78bd4d0de287cac";

    public string CertificatePath { get; set; }
    public string CertificatePassword { get; set; }


    public int HttpListenerDelayMs { get; set; } = 50;
    public string WebHookUrl { get; set; }
}