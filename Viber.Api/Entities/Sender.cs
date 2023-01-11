using Newtonsoft.Json;

namespace Viber.Api.Entities
{
    public class Sender
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("avatar")]
        public string? Avatar { get; set; }
    }
}