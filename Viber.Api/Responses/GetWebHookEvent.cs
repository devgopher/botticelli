using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Viber.Api.Entities;

namespace Viber.Api.Responses
{
    public class GetWebHookEvent
    {
        [JsonPropertyName("event")]
        public string? Event { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("message_token")]
        public long MessageToken { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("desc")]
        public string? Desc { get; set; }

        [JsonPropertyName("sender")]
        public MessageSender? Sender { get; set; }

        [JsonPropertyName("message")]
        public Message? Message { get; set; }
    }
}