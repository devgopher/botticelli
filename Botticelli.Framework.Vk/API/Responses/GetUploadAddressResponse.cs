using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

// VkSendDocumentResponse myDeserializedClass = JsonSerializer.Deserialize<VkSendDocumentResponse>(myJsonResponse);
public class GetUploadAddressResponse
{
    [JsonPropertyName("album_id")] public int AlbumId { get; set; }

    [JsonPropertyName("upload_url")] public string UploadUrl { get; set; }

    [JsonPropertyName("user_id")] public int UserId { get; set; }

    [JsonPropertyName("group_id")] public int GroupId { get; set; }
}