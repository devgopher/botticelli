using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// Prompt template reference and variables used for an assistant request.
/// </summary>
public class Prompt
{
    /// <summary>Unique identifier of the prompt template.</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>Variables substituted into the prompt template.</summary>
    [JsonPropertyName("variables")]
    public object? Variables { get; set; }

    /// <summary>Prompt template version, if versioned.</summary>
    [JsonPropertyName("version")]
    public object? Version { get; set; }

    /// <summary>Indicates whether this prompt block was successfully parsed.</summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}