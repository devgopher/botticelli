using Botticelli.AI.Settings;

namespace Botticelli.AI.YaGpt.Settings;

public class YaGptSettings : AiSettings
{
    public string ApiKey { get; set; }
    public string Model { get; set; }
    public double Temperature { get; set; }
    public string Instruction { get; set; }
    public int MaxTokens { get; set; }
    public bool StreamGeneration { get; set; }
}