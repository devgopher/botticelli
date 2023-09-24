using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Methods;

public class Response
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}