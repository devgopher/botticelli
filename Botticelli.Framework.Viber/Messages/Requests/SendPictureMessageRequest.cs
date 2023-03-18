using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests;

public class SendPictureMessageRequest : SendMediaRequest
{
    [JsonPropertyName("type")]
    public virtual string Type => "picture";
}