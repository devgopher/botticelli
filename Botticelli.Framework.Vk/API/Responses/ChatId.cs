using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class ChatId
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}