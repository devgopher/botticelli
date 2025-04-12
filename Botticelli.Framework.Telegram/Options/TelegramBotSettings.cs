using Botticelli.Framework.Options;

namespace Botticelli.Framework.Telegram.Options;

/// <inheritdoc />
public class TelegramBotSettings : BotSettings
{
    public static string Section => "TelegramBot";

    /// <summary>
    ///     Timeout in ms
    /// </summary>
    public int Timeout { get; set; } = 60000;

    /// <summary>
    ///     Retries count on errors
    /// </summary>
    public int RetryOnFailure { get; set; } = 5;

    /// <summary>
    ///     Use throttling or not?
    /// </summary>
    public bool? UseThrottling { get; set; } = true;

    /// <summary>
    ///     Is this bor autonomous?
    /// </summary>
    public bool? IsAutonomous { get; set; } = false;
    
    /// <summary>
    ///     Should we use test environment
    /// </summary>
    public bool? UseTestEnvironment { get; set; } = false;

    /// <summary>
    ///     Base url for Telegram API
    /// </summary>
    public string? TelegramBaseUrl { get; set; } = "https://api.telegram.org";
}