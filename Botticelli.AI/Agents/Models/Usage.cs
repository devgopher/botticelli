using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// Token usage statistics for a single assistant request/response.
/// </summary>
public class Usage
{
    /// <summary>Number of tokens in the input (prompt + context).</summary>
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }

    /// <summary>Breakdown of input tokens (e.g. cached vs non-cached).</summary>
    [JsonPropertyName("input_tokens_details")]
    public InputTokensDetails InputTokensDetails { get; set; } = new();

    /// <summary>Number of tokens in the generated output.</summary>
    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }

    /// <summary>Breakdown of output tokens (e.g. reasoning vs content).</summary>
    [JsonPropertyName("output_tokens_details")]
    public OutputTokensDetails OutputTokensDetails { get; set; } = new();

    /// <summary>Total tokens (input + output).</summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }

    /// <summary>Indicates whether this usage block was successfully parsed.</summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}