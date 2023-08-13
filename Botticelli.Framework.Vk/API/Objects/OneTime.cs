using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class OneTime
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }
}