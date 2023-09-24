using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Methods;

public class Method
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("access_token_type")]
    public List<string> AccessTokenType { get; set; }

    [JsonPropertyName("parameters")]
    public List<Parameter> Parameters { get; set; }

    [JsonPropertyName("responses")]
    public Responses Responses { get; set; }

    [JsonPropertyName("errors")]
    public List<Error> Errors { get; set; }

    [JsonPropertyName("timeout")]
    public int? Timeout { get; set; }
}