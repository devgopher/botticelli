using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Methods;

public class Items
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("entity")]
    public string Entity { get; set; }

    [JsonPropertyName("minimum")]
    public int Minimum { get; set; }

    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}