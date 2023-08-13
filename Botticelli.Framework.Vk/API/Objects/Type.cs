using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class Type
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("enum")]
    public List<string> Enum { get; set; }

    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}