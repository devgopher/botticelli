using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Viber.Api.Requests
{
    public class RemoveWebHookRequest
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}