using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class VkSendAudioResponse
{
    [JsonPropertyName("response")]
    public VkSendAudioResponseData Response { get; set; }
}