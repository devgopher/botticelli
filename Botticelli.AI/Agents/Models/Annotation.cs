using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// A reference or citation attached to a span of text in assistant output (e.g. link, source).
/// </summary>
public class Annotation
{
    /// <summary>End index of the annotated span in the content text.</summary>
    [JsonPropertyName("end_index")]
    public int EndIndex { get; set; }

    /// <summary>Start index of the annotated span in the content text.</summary>
    [JsonPropertyName("start_index")]
    public int StartIndex { get; set; }

    /// <summary>Display title of the annotation (e.g. link text).</summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>Annotation type (e.g. "url").</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>URL or target of the annotation.</summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>Indicates whether this annotation was successfully parsed.</summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}