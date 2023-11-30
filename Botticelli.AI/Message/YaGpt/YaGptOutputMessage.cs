using System.Text.Json.Serialization;
using Botticelli.AI.Message.ChatGpt;

namespace Botticelli.AI.Message.YaGpt;

public class YaGptOutputMessage
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("object")] public string Object { get; set; }

    [JsonPropertyName("created")] public int Created { get; set; }

    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("usage")] public Usage Usage { get; set; }

    [JsonPropertyName("choices")] public List<Choice> Choices { get; set; }
}