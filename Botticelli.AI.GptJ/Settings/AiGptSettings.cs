using Botticelli.AI.Settings;

namespace Botticelli.AI.GptJ.Settings;

public class AiGptSettings : AiSettings
{
    public string ApiKey { get; set; }
    public int GenerateTokensLimit { get; set; }
    public double TopP { get; set; }
    public int TopK { get; set; }
    public double Temperature { get; set; }
}