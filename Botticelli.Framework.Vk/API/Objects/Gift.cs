using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Objects;

public class Gift
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}