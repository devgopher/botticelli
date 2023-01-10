using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Viber.Api.Entities;

namespace Viber.Api.Requests
{
    public class ApiSendMessageRequest
    {
        [JsonPropertyName("receiver")]
        public string Receiver { get; set; }

        [JsonPropertyName("min_api_version")]
        public int MinApiVersion { get; set; }

        [JsonPropertyName("sender")]
        public Sender Sender { get; set; }

        [JsonPropertyName("tracking_data")]
        public string TrackingData { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}