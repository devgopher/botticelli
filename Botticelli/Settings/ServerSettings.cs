using FluentEmail.MailKitSmtp;

namespace Botticelli.Server.Settings;

public class ServerSettings
{
    public ServerSettings(string botInfoDb)
    {
        BotInfoDb = botInfoDb;
    }

    /// <summary>
    ///     Token lifetime minutes
    /// </summary>
    public int TokenLifetimeMin { get; set; }

    public required SmtpClientOptions SmtpClientOptions { get; set; }

    public required string ServerEmail { get; set; }

    public int HttpsPort { get; set; } 
    public required string ServerUrl { get; set; }
    public string? AnalyticsUrl { get; set; }
    public required string SecureStorageConnection { get; set; }
    public bool UseSsl { get; set; }

    public string BotInfoDb { get; set; }
}