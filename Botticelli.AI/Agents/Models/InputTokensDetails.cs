using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// Breakdown of input token usage (e.g. cached vs non-cached tokens).
/// </summary>
public class InputTokensDetails
{
    /// <summary>Number of input tokens served from cache, if applicable.</summary>
    [JsonPropertyName("cached_tokens")]
    public int CachedTokens { get; set; }

    /// <summary>Indicates whether this details block was successfully parsed.</summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}