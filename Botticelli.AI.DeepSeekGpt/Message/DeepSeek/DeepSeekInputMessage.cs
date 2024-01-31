using System.Text.Json.Serialization;

namespace Botticelli.AI.DeepSeekGpt.Message.DeepSeek;

public class DeepSeekInputMessage
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("messages")]
    public List<DeepSeekInnerInputMessage> Messages { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("maxTokens")]
    public int MaxTokens { get; set; }
}

public class DeepSeekInnerInputMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}