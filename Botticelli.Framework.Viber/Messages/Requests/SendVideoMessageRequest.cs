using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests;

public class SendVideoMessageRequest : SendMediaRequest
{
    [JsonPropertyName("type")]
    public virtual string Type => "video";

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }
}