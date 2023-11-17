using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class UploadDocResult
{
    [JsonPropertyName("server")] public int Server { get; set; }

    [JsonPropertyName("file")] public string File { get; set; }

    [JsonPropertyName("hash")] public string Hash { get; set; }
}