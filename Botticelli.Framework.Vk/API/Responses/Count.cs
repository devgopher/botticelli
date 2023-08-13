using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class Count
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("minimum")]
    public int Minimum { get; set; }
}