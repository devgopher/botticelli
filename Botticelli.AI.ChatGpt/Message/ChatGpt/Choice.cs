using System.Text.Json.Serialization;

namespace Botticelli.AI.ChatGpt.Message.ChatGpt;

public class Choice
{
    [JsonPropertyName("message")] public ChatGptMessage? Message { get; set; }

    [JsonPropertyName("delta")] public ChatGptMessage? Delta { get; set; }
    [JsonPropertyName("finish_reason")] public string? FinishReason { get; set; }

    [JsonPropertyName("index")] public int Index { get; set; }
}