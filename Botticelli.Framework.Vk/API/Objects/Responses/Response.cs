using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Responses;

public class Response
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; }

    [JsonPropertyName("additionalProperties")]
    public bool AdditionalProperties { get; set; }

    [JsonPropertyName("items")]
    public Items Items { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}