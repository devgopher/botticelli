using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class Call
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("enum")]
    public List<string> Enum { get; set; }

    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}