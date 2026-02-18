using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// A single content block within an assistant output (e.g. text with optional annotations).
/// </summary>
public class ContentItem
{
    /// <summary>Optional references or citations attached to this content (e.g. URLs, titles).</summary>
    [JsonPropertyName("annotations")]
    public List<Annotation>? Annotations { get; set; }

    /// <summary>Plain text content of this block.</summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    /// <summary>Content block type (e.g. "text").</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    // [JsonPropertyName("logprobs")]
    // public object? Logprobs { get; set; }

    /// <summary>Indicates whether this item was successfully parsed from the API response.</summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}