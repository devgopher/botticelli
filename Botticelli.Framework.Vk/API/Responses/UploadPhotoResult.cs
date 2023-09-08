using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class UploadPhotoResult
{
    [JsonPropertyName("response")]
    public List<UploadPhotoResponse> Response { get; set; }
}