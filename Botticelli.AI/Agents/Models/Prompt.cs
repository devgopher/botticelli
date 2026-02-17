using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class Prompt
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("variables")]
    public object? Variables { get; set; }

    [JsonPropertyName("version")]
    public object? Version { get; set; }

    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}