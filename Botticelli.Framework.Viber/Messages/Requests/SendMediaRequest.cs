using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests;

public abstract class SendMediaRequest : SendTextMessageRequest
{
    [JsonPropertyName("media")]
    public string Media { get; set; }

    [JsonPropertyName("thumbnail")]
    public string Thumbnail { get; set; }
}