using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Responses;

public class Events
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}