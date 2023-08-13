using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class TranscriptError
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("minimum")]
    public int Minimum { get; set; }

    [JsonPropertyName("maximum")]
    public int Maximum { get; set; }
}