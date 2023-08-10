using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods;

public class BanInfo
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}