using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Objects;

public class Type
{
    [JsonPropertyName("type")]
    public string Type1 { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("enum")]
    public List<string> Enum { get; set; }

    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}