using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// Tool configuration available to the assistant (e.g. web search, context size, user location).
/// </summary>
public class Tool
{
    /// <summary>Tool type (e.g. "web_search").</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Size of search context to use (e.g. number of results or tokens).</summary>
    [JsonPropertyName("search_context_size")]
    public string SearchContextSize { get; set; } = string.Empty;

    /// <summary>User location context for this tool, if applicable.</summary>
    [JsonPropertyName("user_location")]
    public UserLocation UserLocation { get; set; } = new();

    /// <summary>Indicates whether this tool was successfully parsed.</summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}