using Botticelli.Framework.Options;

namespace Botticelli.Framework.Vk.Messages.Options;

/// <inheritdoc />
public class VkBotSettings : BotSettings
{
    public static string Section => "VkBot";

    public int PollIntervalMs { get; set; } = 500;
    public int GroupId { get; set; }
}