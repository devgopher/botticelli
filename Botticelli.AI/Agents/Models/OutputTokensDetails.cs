using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class OutputTokensDetails
{
    [JsonPropertyName("reasoning_tokens")]
    public int ReasoningTokens { get; set; }

    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}