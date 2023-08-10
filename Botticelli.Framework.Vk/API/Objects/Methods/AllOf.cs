using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods;

public class AllOf
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; }
}