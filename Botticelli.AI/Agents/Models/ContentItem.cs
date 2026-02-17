using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class ContentItem
{
    [JsonPropertyName("annotations")]
    public List<Annotation>? Annotations { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    // [JsonPropertyName("logprobs")]
    // public object? Logprobs { get; set; }

    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}