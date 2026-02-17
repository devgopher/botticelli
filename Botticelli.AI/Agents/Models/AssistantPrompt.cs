using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class AssistantPrompt
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}