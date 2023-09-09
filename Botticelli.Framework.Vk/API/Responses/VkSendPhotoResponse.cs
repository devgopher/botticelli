using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class VkSendPhotoResponse
{
    [JsonPropertyName("response")]
    public List<VkSendPhotoPartialResponse> Response { get; set; }
}