using System.Text.Json.Serialization;

namespace Botticelli.AI.Message.ChatGpt;

public class ChatGptInputMessage
{
    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("messages")] public List<ChatGptMessage> Messages { get; set; }

    [JsonPropertyName("temperature")] public double Temperature { get; set; }
}