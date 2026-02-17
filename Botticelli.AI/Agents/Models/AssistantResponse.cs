using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

public class AssistantResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public double CreatedAt { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("incomplete_details")]
    public object? IncompleteDetails { get; set; }

    [JsonPropertyName("instructions")]
    public string Instructions { get; set; } = string.Empty;

    // [JsonPropertyName("metadata")]
    // public object? Metadata { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("output")]
    public List<OutputItem> Output { get; set; } = new();

    [JsonPropertyName("parallel_tool_calls")]
    public bool ParallelToolCalls { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("tool_choice")]
    public string ToolChoice { get; set; } = string.Empty;

    [JsonPropertyName("tools")]
    public List<Tool> Tools { get; set; } = new();

    [JsonPropertyName("top_p")]
    public object? TopP { get; set; }

    [JsonPropertyName("background")]
    public bool Background { get; set; }

    [JsonPropertyName("max_output_tokens")]
    public int? MaxOutputTokens { get; set; }

    // [JsonPropertyName("max_tool_calls")]
    // public object? MaxToolCalls { get; set; }

    [JsonPropertyName("previous_response_id")]
    public object? PreviousResponseId { get; set; }

    [JsonPropertyName("prompt")]
    public Prompt Prompt { get; set; } = new();

    // [JsonPropertyName("reasoning")]
    // public object? Reasoning { get; set; }
    //
    // [JsonPropertyName("service_tier")]
    // public object? ServiceTier { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    // [JsonPropertyName("top_logprobs")]
    // public object? TopLogprobs { get; set; }
    //
    // [JsonPropertyName("truncation")]
    // public object? Truncation { get; set; }

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; } = new();

    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}