using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class AssistantRequest
{
    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [JsonPropertyName("input")]
    public required string Input { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("max_output_tokens")]
    public int MaxOutputTokens { get; set; }
}