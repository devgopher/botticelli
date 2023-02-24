namespace Botticelli.AI.Settings;

public class AiBotSettings
{
    public AiBotSettings() => Settings = new List<AiSettings>(5);

    public string SelectedAiName { get; set; }

    public IEnumerable<AiSettings> Settings { get; set; }
}