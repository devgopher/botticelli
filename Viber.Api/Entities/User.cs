using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Callbacks.Models;

public class User
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("avatar")]
    public string Avatar { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("api_version")]
    public int ApiVersion { get; set; }
}