using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// A single output unit in an assistant response (e.g. a message or tool-call result).
/// </summary>
public class OutputItem
{
    /// <summary>Unique identifier of this output item.</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>Optional web search or other tool action associated with this item.</summary>
    [JsonPropertyName("action")]
    public WebSearchAction? Action { get; set; }

    /// <summary>Status of this output (e.g. "success", "in_progress").</summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>Output type (e.g. "message").</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Indicates whether this item was successfully parsed.</summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }

    /// <summary>Content blocks (e.g. text with annotations) for message-type output.</summary>
    [JsonPropertyName("content")]
    public List<ContentItem>? Content { get; set; }

    /// <summary>Role of the sender (e.g. "assistant").</summary>
    [JsonPropertyName("role")]
    public string? Role { get; set; }
}