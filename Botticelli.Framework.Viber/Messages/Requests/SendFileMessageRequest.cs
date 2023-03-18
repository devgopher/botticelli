using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests;

public class SendFileMessageRequest : SendMediaRequest
{
    [JsonPropertyName("type")]
    public virtual string Type => "File";

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }
}