using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Viber.Api.Requests
{
    public class SetWebHookRequest
    {
        [JsonPropertyName("auth_token")]
        public string AuthToken { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("event_types")]
        public List<string> EventTypes { get; set; }

        [JsonPropertyName("send_name")]
        public bool SendName { get; set; }

        [JsonPropertyName("send_photo")]
        public bool SendPhoto { get; set; }
    }
}
