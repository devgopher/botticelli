using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class Credentials
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}