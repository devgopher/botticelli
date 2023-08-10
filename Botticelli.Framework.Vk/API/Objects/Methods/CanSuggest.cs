using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods;

public class CanSuggest
{
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}