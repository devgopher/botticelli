using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class OutputItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("action")]
    public WebSearchAction? Action { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("valid")]
    public bool Valid { get; set; }

    [JsonPropertyName("content")]
    public List<ContentItem>? Content { get; set; }

    [JsonPropertyName("role")]
    public string? Role { get; set; }
}