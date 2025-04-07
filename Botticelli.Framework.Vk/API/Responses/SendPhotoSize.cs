using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class SendPhotoSize
{
    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}