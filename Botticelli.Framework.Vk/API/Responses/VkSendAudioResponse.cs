using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class VkSendAudioResponse
{
    [JsonPropertyName("response")]
    public VkSendAudioResponseData Response { get; set; }
}