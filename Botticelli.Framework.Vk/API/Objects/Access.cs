using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Access
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
