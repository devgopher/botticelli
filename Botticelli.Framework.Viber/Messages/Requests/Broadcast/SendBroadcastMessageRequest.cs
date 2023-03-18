using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests.Broadcast
{
    public class SendBroadcastMessageRequest
    {
        [JsonPropertyName("sender")]
        public Sender Sender { get; set; }

        [JsonPropertyName("min_api_version")]
        public int MinApiVersion { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("broadcast_list")]
        public List<string> BroadcastList { get; set; }

        [JsonPropertyName("rich_media")]
        public RichMedia RichMedia { get; set; }
    }
}
