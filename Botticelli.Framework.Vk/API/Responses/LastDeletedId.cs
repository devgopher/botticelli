using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class LastDeletedId
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("minimum")]
    public int Minimum { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }
}