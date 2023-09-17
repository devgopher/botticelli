using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class VkSendVideoResponse
{
    [JsonPropertyName("response")]
    public VkSendVideoResponseData Response { get; set; }
}