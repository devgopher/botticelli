using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods;

public class Root
{
    [JsonPropertyName("$schema")]
    public string Schema { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("definitions")]
    public Definitions Definitions { get; set; }
}