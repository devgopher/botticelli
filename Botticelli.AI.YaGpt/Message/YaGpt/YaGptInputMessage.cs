using System.Text.Json.Serialization;
// ReSharper disable CollectionNeverQueried.Global

namespace Botticelli.AI.YaGpt.Message.YaGpt;

public class YaGptInputMessage
{
    [JsonPropertyName("model_uri")]
    public required string ModelUri { get; set; }

    [JsonPropertyName("completionOptions")]
    public required CompletionOptions CompletionOptions { get; set; }

    [JsonPropertyName("messages")]
    public required List<YaGptMessage> Messages { get; set; }
}