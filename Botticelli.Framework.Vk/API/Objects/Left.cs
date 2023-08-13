using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class Left
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}