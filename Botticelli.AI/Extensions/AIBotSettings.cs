namespace Botticelli.AI.Extensions;

public class AIBotSettings
{
    public AIBotSettings() => Settings = new List<AISettings>(5);

    public List<AISettings> Settings { get; set; }
}