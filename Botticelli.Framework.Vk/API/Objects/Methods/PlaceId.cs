using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods;

public class PlaceId
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}