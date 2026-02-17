using System.Text.Json.Serialization;

namespace Botticelli.AI.YaGpt.Message.YaGpt;

public class CompletionOptions
{
    [JsonPropertyName("stream")]
    public bool Stream { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }
}