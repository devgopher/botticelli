using System.Text.Json.Serialization;

namespace Viber.Api.Responses
{
    public class ApiSendMessageResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("status_message")]
        public string? StatusMessage { get; set; }

        [JsonPropertyName("message_token")]
        public long MessageToken { get; set; }

        [JsonPropertyName("chat_hostname")]
        public string? ChatHostname { get; set; }

        [JsonPropertyName("billing_status")]
        public int BillingStatus { get; set; }
    }
}