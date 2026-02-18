using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// Request payload sent to the AI assistant API (e.g. Yandex GPT).
/// </summary>
public class AssistantRequest
{
    /// <summary>Model identifier to use for generation.</summary>
    [JsonPropertyName("model")]
    public required string Model { get; set; }

    /// <summary>User input text (message body) to process.</summary>
    [JsonPropertyName("input")]
    public required string Input { get; set; }

    /// <summary>Sampling temperature for response randomness (0–1).</summary>
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    /// <summary>Maximum number of tokens to generate in the response.</summary>
    [JsonPropertyName("max_output_tokens")]
    public int MaxOutputTokens { get; set; }
}