using Botticelli.Framework.Options;

namespace Botticelli.Framework.Facebook.Messages.Options;

/// <inheritdoc />
public class FacebookBotSettings : BotSettings
{
    public string? AppSecret { get; set; }
    public string? VerifyToken { get; set; }
}