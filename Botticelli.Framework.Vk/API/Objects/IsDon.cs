using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Objects;

public class IsDon
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}