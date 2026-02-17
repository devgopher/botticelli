using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class Tool
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("search_context_size")]
    public string SearchContextSize { get; set; } = string.Empty;

    [JsonPropertyName("user_location")]
    public UserLocation UserLocation { get; set; } = new();

    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}