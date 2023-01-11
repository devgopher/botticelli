using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Viber.Api.Requests
{
    public class SetWebHookRequest : BaseRequest
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("event_types")]
        public List<string>? EventTypes { get; set; }

        [JsonPropertyName("send_name")]
        public bool SendName { get; set; }

        [JsonPropertyName("send_photo")]
        public bool SendPhoto { get; set; }
    }
}
