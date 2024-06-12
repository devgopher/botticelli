using Botticelli.Framework.Options;

namespace Botticelli.Framework.Telegram.Options;

/// <inheritdoc />
public class TelegramBotSettings : BotSettings
{
    /// <summary>
    /// Timeout in ms
    /// </summary>
    public int Timeout { get; set; } = 60000;
}