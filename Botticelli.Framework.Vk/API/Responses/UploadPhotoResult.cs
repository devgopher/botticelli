using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class UploadPhotoResult
{
    [JsonPropertyName("server")] public int Server { get; set; }

    [JsonPropertyName("photo")] public string Photo { get; set; }

    [JsonPropertyName("hash")] public string Hash { get; set; }
}