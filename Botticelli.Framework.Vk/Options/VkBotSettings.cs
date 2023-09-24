using Botticelli.Framework.Options;

namespace Botticelli.Framework.Vk.Messages.Options;

/// <inheritdoc />
public class VkBotSettings : BotSettings
{
    public VkBotSettings() => PollIntervalMs = 500;
    public int PollIntervalMs { get; set; }
    public int GroupId { get; set; }
}