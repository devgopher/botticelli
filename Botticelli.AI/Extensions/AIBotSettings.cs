namespace Botticelli.AI.Extensions;

public class AIBotSettings
{
    public AIBotSettings() => Settings = new(5);

    public List<AISettings> Settings { get; set; }
}