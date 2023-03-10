using Botticelli.Framework.Options;

namespace Botticelli.Framework.Telegram.Options;

/// <inheritdoc />
public class TelegramBotSettings : BotSettings
{
    /// <summary>
    ///     Chat polling interval
    /// </summary>
    public int ChatPollingIntervalMs { get; set; }
}