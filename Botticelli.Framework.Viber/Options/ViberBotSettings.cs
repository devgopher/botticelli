using Botticelli.Framework.Options;

namespace Botticelli.Framework.Viber.Options;

/// <inheritdoc />
public class ViberBotSettings : BotSettings
{
    /// <summary>
    ///     Viber API token
    /// </summary>
    public string ViberToken { get; set; }
}