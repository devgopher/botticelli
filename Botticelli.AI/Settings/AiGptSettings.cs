namespace Botticelli.AI.Settings;

public class AiGptSettings
{
    public int GenerateTokensLimit { get; set; }
    public double TopP { get; set; }
    public int TopK { get; set; }
    public double Temperature { get; set; }
}