using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class Messages
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }
}