using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// Represents a web search action invoked by the assistant (e.g. query and type).
/// </summary>
public class WebSearchAction
{
    /// <summary>Search query that was executed.</summary>
    [JsonPropertyName("query")]
    public string Query { get; set; } = string.Empty;

    /// <summary>Action type (e.g. "web_search").</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Indicates whether this action was successfully parsed.</summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}