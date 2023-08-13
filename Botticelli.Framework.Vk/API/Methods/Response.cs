using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Methods;

public class Response
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}