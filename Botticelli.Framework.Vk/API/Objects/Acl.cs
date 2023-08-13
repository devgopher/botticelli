using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class Acl
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }
}