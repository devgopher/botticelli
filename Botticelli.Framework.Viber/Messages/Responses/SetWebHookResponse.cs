using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Responses
{
    public class SetWebHookResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("status_message")]
        public string StatusMessage { get; set; }

        [JsonPropertyName("event_types")]
        public List<string> EventTypes { get; set; }
    }
}
