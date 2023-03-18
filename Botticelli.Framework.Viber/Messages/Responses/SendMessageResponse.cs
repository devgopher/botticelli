using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Responses;

/// <summary>
/// Send response message
/// </summary>
public class SendMessageResponse
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("status_message")]
    public string StatusMessage { get; set; }

    [JsonPropertyName("message_token")]
    public long MessageToken { get; set; }

    [JsonPropertyName("chat_hostname")]
    public string ChatHostname { get; set; }

    [JsonPropertyName("billing_status")]
    public int BillingStatus { get; set; }
}