namespace Botticelli.AI.Extensions;

public class AiBotSettings
{
    public AiBotSettings() => Settings = new List<AiSettings>(5);

    public List<AiSettings> Settings { get; set; }
}