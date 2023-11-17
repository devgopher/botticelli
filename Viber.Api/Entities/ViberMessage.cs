using Newtonsoft.Json;

namespace Viber.Api.Entities
{
    public class ViberMessage
    {
        [JsonProperty("type")] public string? Type { get; set; }

        [JsonProperty("text")] public string? Text { get; set; }

        [JsonProperty("media")] public string? Media { get; set; }

        [JsonProperty("location")] public Location? Location { get; set; }

        [JsonProperty("tracking_data")] public string? TrackingData { get; set; }
    }
}