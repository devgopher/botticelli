using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests;

public class SendUrlMessageRequest : SendMediaRequest
{
    [JsonPropertyName("type")]
    public override string Type => "url";
}