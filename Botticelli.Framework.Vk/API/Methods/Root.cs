using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Methods;

public class Root
{
    [JsonPropertyName("$schema")]
    public string Schema { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("termsOfService")]
    public string TermsOfService { get; set; }

    [JsonPropertyName("methods")]
    public List<Method> Methods { get; set; }
}