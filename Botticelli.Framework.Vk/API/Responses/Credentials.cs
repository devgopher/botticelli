using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class Credentials
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}