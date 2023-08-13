using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class UnreadCount
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("minimum")]
    public int Minimum { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}