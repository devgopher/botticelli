namespace Botticelli.Framework.Options;

/// <inheritdoc />
public class TelegramBotSettings : BotSettings
{
    /// <summary>
    ///     Chat polling interval
    /// </summary>
    public int ChatPollingIntervalMs { get; set; }

    /// <summary>
    ///     Telegram API token
    /// </summary>
    public string TelegramToken { get; set; }
}