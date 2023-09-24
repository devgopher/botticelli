using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Objects;

public class LocalId
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}