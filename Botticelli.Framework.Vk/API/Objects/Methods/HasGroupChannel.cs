using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods;

public class HasGroupChannel
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}