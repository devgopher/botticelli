using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class AudioResponseData
{
    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("audio_message")] public AudioMessage AudioMessage { get; set; }
}