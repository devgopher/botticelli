using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods;

public class IsClosed
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}