using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods;

public class GroupsSectionsListItem
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("maxItems")]
    public int MaxItems { get; set; }

    [JsonPropertyName("minItems")]
    public int MinItems { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("items")]
    public Items Items { get; set; }
}