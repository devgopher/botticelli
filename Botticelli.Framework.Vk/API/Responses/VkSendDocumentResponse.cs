using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class VkSendDocumentResponse
{
    [JsonPropertyName("response")] public DocumentResponseData DocumentResponseData { get; set; }
}