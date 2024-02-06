using System.Text.Json.Serialization;

namespace Botticelli.AI.YaGpt.Message.YaGpt;

public class Alternative
{
    [JsonPropertyName("message")] public Message Message { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }
}

public class Message
{
    [JsonPropertyName("role")] public string Role { get; set; }

    [JsonPropertyName("text")] public string Text { get; set; }
}

public class Result
{
    [JsonPropertyName("alternatives")] public List<Alternative> Alternatives { get; set; }

    [JsonPropertyName("usage")] public Usage Usage { get; set; }

    [JsonPropertyName("modelVersion")] public string ModelVersion { get; set; }
}

public class YaGptOutputMessage
{
    [JsonPropertyName("result")] public Result Result { get; set; }
}

public class Usage
{
    [JsonPropertyName("inputTextTokens")] public string InputTextTokens { get; set; }

    [JsonPropertyName("completionTokens")] public string CompletionTokens { get; set; }

    [JsonPropertyName("totalTokens")] public string TotalTokens { get; set; }
}