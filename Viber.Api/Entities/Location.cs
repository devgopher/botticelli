using Newtonsoft.Json;

namespace Viber.Api.Entities
{
    public class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }
    }
}