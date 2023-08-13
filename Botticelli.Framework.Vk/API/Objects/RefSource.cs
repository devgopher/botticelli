using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class RefSource
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}