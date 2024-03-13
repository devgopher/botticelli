namespace Botticelli.AI.Settings;

public class AiSettings : ProviderSettings
{
    public string Url { get; set; }
    public string AiName { get; set; }
    public bool StreamGeneration { get; set; }
}