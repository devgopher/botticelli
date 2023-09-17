using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class UploadVideoResult
{
    [JsonPropertyName("server")]
    public int Server { get; set; }

    [JsonPropertyName("video")]
    public string Video { get; set; }

    [JsonPropertyName("hash")]
    public string Hash { get; set; }
}