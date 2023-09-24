using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class IsAllowed
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}