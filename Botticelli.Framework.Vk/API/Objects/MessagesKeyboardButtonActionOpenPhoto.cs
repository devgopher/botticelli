using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class MessagesKeyboardButtonActionOpenPhoto
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; }

    [JsonPropertyName("additionalProperties")]
    public bool AdditionalProperties { get; set; }
}