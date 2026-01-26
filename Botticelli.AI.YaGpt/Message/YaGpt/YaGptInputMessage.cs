using System.Text.Json.Serialization;

namespace Botticelli.AI.YaGpt.Message.YaGpt;

public class YaGptInputMessage
{
    [JsonPropertyName("model_uri")]
    public string ModelUri { get; set; }

    [JsonPropertyName("completionOptions")]
    public CompletionOptions CompletionOptions { get; set; }

    [JsonPropertyName("messages")]
    public List<YaGptMessage> Messages { get; set; }
}

public class CompletionOptions
{
    [JsonPropertyName("stream")]
    public bool Stream { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }
}

public class YaGptMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}