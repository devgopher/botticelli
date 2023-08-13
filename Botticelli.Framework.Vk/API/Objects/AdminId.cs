using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class AdminId
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("entity")]
    public string Entity { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("minimum")]
    public int Minimum { get; set; }
}