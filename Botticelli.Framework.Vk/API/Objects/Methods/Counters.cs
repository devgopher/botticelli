using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods;

public class Counters
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}