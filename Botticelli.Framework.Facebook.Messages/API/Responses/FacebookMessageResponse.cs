using System.Text.Json.Serialization;

namespace Botticelli.Framework.Facebook.Messages.API.Requests;

public class FacebookMessageResponse
{
    [JsonPropertyName("recipient_id")]
    public string? RecipientId { get; set; }

    [JsonPropertyName("message_id")]
    public string? MessageId { get; set; }
}