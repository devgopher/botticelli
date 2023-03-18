using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests;

public class SendContactMessageRequest : SendMediaRequest
{
    [JsonPropertyName("type")]
    public override string Type => "contact";

    [JsonPropertyName("contact")]
    public Contact Contact { get; set; }
}