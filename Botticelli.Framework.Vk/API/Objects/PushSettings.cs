using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class PushSettings
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}