using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Responses;

public class EventGroupId
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}