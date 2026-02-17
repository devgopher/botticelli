namespace Botticelli.AI.Settings;

public class AgentSettings
{
    public required string Url { get; set; }
    public required string AiName { get; set; }
    public string? AuthMethod { get; set; } = "Bearer";
    public required string ApiKey { get; set; }
    public string Model { get; set; }
    public double Temperature { get; set; }
    public int MaxOutputTokens { get; set; }
    public TimeSpan? Timeout { get; set; } = TimeSpan.FromMinutes(1);
}