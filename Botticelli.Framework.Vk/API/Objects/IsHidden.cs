using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Objects;

public class IsHidden
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}