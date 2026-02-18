using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// Reference to a prompt template used for the assistant request.
/// </summary>
public class AssistantPrompt
{
    /// <summary>Unique identifier of the prompt.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}