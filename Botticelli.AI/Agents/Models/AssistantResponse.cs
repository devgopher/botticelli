using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// Response returned by the AI assistant API, containing generated output and metadata.
/// </summary>
public class AssistantResponse
{
    /// <summary>Unique identifier of the response.</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>Unix timestamp when the response was created.</summary>
    [JsonPropertyName("created_at")]
    public double CreatedAt { get; set; }

    /// <summary>Error message from the API, if any.</summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    /// <summary>Details when the response was truncated or incomplete.</summary>
    [JsonPropertyName("incomplete_details")]
    public object? IncompleteDetails { get; set; }

    /// <summary>Instructions or system prompt applied to this run.</summary>
    [JsonPropertyName("instructions")]
    public string Instructions { get; set; } = string.Empty;

    // [JsonPropertyName("metadata")]
    // public object? Metadata { get; set; }

    /// <summary>Model that produced this response.</summary>
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>API object type (e.g. "response").</summary>
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    /// <summary>Generated output items (e.g. message content).</summary>
    [JsonPropertyName("output")]
    public List<OutputItem> Output { get; set; } = new();

    /// <summary>Whether parallel tool calls were allowed.</summary>
    [JsonPropertyName("parallel_tool_calls")]
    public bool ParallelToolCalls { get; set; }

    /// <summary>Temperature used for this run.</summary>
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    /// <summary>Tool choice mode (e.g. "auto", "none").</summary>
    [JsonPropertyName("tool_choice")]
    public string ToolChoice { get; set; } = string.Empty;

    /// <summary>Tools available to the model (e.g. web search).</summary>
    [JsonPropertyName("tools")]
    public List<Tool> Tools { get; set; } = new();

    /// <summary>Top-p (nucleus) sampling parameter.</summary>
    [JsonPropertyName("top_p")]
    public object? TopP { get; set; }

    /// <summary>Whether the request was processed in background.</summary>
    [JsonPropertyName("background")]
    public bool Background { get; set; }

    /// <summary>Maximum output tokens for this run.</summary>
    [JsonPropertyName("max_output_tokens")]
    public int? MaxOutputTokens { get; set; }

    // [JsonPropertyName("max_tool_calls")]
    // public object? MaxToolCalls { get; set; }

    /// <summary>ID of the previous response in a chain, if any.</summary>
    [JsonPropertyName("previous_response_id")]
    public object? PreviousResponseId { get; set; }

    /// <summary>Prompt and variables used for this request.</summary>
    [JsonPropertyName("prompt")]
    public Prompt Prompt { get; set; } = new();

    // [JsonPropertyName("reasoning")]
    // public object? Reasoning { get; set; }
    //
    // [JsonPropertyName("service_tier")]
    // public object? ServiceTier { get; set; }

    /// <summary>Status of the response (e.g. "success", "in_progress").</summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>Convenience text field; primary content is usually in <see cref="Output"/>.</summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    // [JsonPropertyName("top_logprobs")]
    // public object? TopLogprobs { get; set; }
    //
    // [JsonPropertyName("truncation")]
    // public object? Truncation { get; set; }

    /// <summary>Token usage statistics for this request.</summary>
    [JsonPropertyName("usage")]
    public Usage Usage { get; set; } = new();

    /// <summary>User or session identifier from the API.</summary>
    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    /// <summary>Indicates whether this response was successfully parsed.</summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}