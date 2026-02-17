using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class WebSearchAction
{
    [JsonPropertyName("query")]
    public string Query { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}