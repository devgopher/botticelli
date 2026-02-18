using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// Breakdown of output token usage (e.g. reasoning vs content tokens).
/// </summary>
public class OutputTokensDetails
{
    /// <summary>Number of tokens used for reasoning or chain-of-thought, if reported.</summary>
    [JsonPropertyName("reasoning_tokens")]
    public int ReasoningTokens { get; set; }

    /// <summary>Indicates whether this details block was successfully parsed.</summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}