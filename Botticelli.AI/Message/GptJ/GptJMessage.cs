using Newtonsoft.Json;

namespace Botticelli.AI.Message.GptJ;

public class GptJMessage
{
    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("generate_tokens_limit")]
    public int GenerateTokensLimit { get; set; }

    [JsonProperty("top_p")]
    public double TopP { get; set; }

    [JsonProperty("top_k")]
    public int TopK { get; set; }

    [JsonProperty("temperature")]
    public double Temperature { get; set; }
}