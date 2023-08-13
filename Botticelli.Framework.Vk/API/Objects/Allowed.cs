using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class Allowed
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }
}