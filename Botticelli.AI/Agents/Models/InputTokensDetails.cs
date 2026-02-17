using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class InputTokensDetails
{
    [JsonPropertyName("cached_tokens")]
    public int CachedTokens { get; set; }

    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}