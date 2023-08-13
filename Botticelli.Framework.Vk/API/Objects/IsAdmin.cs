using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class IsAdmin
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}