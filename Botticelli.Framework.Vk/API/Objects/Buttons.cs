using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class Buttons
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("items")]
    public Items Items { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }
}