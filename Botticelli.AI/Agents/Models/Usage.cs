using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class Usage
{
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }

    [JsonPropertyName("input_tokens_details")]
    public InputTokensDetails InputTokensDetails { get; set; } = new();

    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }

    [JsonPropertyName("output_tokens_details")]
    public OutputTokensDetails OutputTokensDetails { get; set; } = new();

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }

    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}