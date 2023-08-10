using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class ObsceneWords
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("items")]
    public Items Items { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }
}