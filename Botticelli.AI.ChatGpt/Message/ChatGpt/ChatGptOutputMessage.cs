// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

using System.Text.Json.Serialization;

namespace Botticelli.AI.ChatGpt.Message.ChatGpt;

public class ChatGptOutputMessage
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("object")] public string Object { get; set; }

    [JsonPropertyName("created")] public int Created { get; set; }

    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("usage")] public Usage Usage { get; set; }

    [JsonPropertyName("choices")] public List<Choice> Choices { get; set; }
}