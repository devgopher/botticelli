using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class VkSendVideoResponseData
{
    [JsonPropertyName("access_key")]
    public string AccessKey { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("owner_id")]
    public int OwnerId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("upload_url")]
    public string UploadUrl { get; set; }

    [JsonPropertyName("video_id")]
    public int VideoId { get; set; }
}