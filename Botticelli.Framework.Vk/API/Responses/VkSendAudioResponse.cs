using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class VkSendAudioResponse
{
    [JsonPropertyName("response")]
    public AudioResponseData AudioResponseData { get; set; }
}



// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class AudioMessage
{
    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("link_mp3")]
    public string LinkMp3 { get; set; }

    [JsonPropertyName("link_ogg")]
    public string LinkOgg { get; set; }

    [JsonPropertyName("owner_id")]
    public int OwnerId { get; set; }

    [JsonPropertyName("access_key")]
    public string AccessKey { get; set; }

    [JsonPropertyName("waveform")]
    public List<int> Waveform { get; set; }
}

public class AudioResponseData
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("audio_message")]
    public AudioMessage AudioMessage { get; set; }
}
