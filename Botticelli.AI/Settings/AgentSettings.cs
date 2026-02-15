namespace Botticelli.AI.Settings;

public class AgentSettings
{
    public required string Url { get; set; }
    public required string AiName { get; set; }
    public string? AuthMethod { get; set; } = "Bearer";
    public required string ApiKey { get; set; }
}