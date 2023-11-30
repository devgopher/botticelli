using System.Text.Json.Serialization;

namespace Botticelli.AI.Message.YaGpt;

public class YaGptInputMessage
{
    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("instruction_text")] public string InstructionText { get; set; }
    [JsonPropertyName("request_text")] public string RequestText { get; set; }
    [JsonPropertyName("generation_options")] public GenerationOptions GenerationOptions { get; set; }

}

public class GenerationOptions
{
    [JsonPropertyName("temperature")] public double Temperature { get; set; }
    [JsonPropertyName("max_tokens")] public int MaxTokens { get; set; }
}