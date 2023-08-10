using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class PublicDateLabel
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}