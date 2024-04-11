using System.Text.Json.Serialization;

namespace Botticelli.Framework.Facebook.Messages.API.Requests;

public class FacebookMessageRequest
{
    [JsonPropertyName("recipient")] public Recipient? Recipient { get; set; }
    [JsonPropertyName("messaging_type")] public string? MessagingType { get; set; }
    [JsonPropertyName("message")] public FbMessage? Message { get; set; }
    [JsonPropertyName("access_token")] public string? AccessToken { get; set; }
}