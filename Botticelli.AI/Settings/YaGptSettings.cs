namespace Botticelli.AI.Settings;

public class YaGptSettings : AiSettings
{
    public string ApiKey { get; set; }
    public string Model { get; set; }
    public double Temperature { get; set; }
    public string Role { get; set; }
    public int MaxTokens { get; set; }
}