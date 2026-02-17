using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class Annotation
{
    [JsonPropertyName("end_index")]
    public int EndIndex { get; set; }

    [JsonPropertyName("start_index")]
    public int StartIndex { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}