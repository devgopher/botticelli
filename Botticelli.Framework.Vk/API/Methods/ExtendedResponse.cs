using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Methods;

public class ExtendedResponse
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}