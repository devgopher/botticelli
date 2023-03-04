using System.Text.Json.Serialization;

namespace Botticelli.AI.Message.GptJ;

public class GptJInputMessage
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("generate_tokens_limit")]
    public int GenerateTokensLimit { get; set; }

    [JsonPropertyName("top_p")]
    public double TopP { get; set; }

    [JsonPropertyName("top_k")]
    public int TopK { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
}