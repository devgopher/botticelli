using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages
{
    /// <summary>
    /// Callback request (comes from Viber).
    /// Expected response only: 200 - OK.
    /// In other cases bot isn't available for Viber 
    /// </summary>
    public class CallbackIncomingRequest
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("message_token")]
        public long MessageToken { get; set; }
    }
}
