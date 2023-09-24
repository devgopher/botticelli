using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Methods;

public class Parameter
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("minimum")]
    public int Minimum { get; set; }

    [JsonPropertyName("maximum")]
    public int Maximum { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("entity")]
    public string Entity { get; set; }

    [JsonPropertyName("maxLength")]
    public int? MaxLength { get; set; }

    [JsonPropertyName("items")]
    public Items Items { get; set; }

    [JsonPropertyName("maxItems")]
    public int? MaxItems { get; set; }

    [JsonPropertyName("default")]
    public object Default { get; set; }

    [JsonPropertyName("enum")]
    public List<object> Enum { get; set; }

    [JsonPropertyName("enumNames")]
    public List<string> EnumNames { get; set; }

    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}