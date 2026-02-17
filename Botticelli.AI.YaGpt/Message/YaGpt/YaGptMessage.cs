using System.Text.Json.Serialization;

namespace Botticelli.AI.YaGpt.Message.YaGpt;

public class YaGptMessage
{
    [JsonPropertyName("role")]
    public required string Role { get; set; }

    [JsonPropertyName("text")]
    public required string Text { get; set; }
}