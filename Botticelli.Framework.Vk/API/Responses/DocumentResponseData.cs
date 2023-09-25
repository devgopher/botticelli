using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class DocumentResponseData
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("doc")]
    public Document Document { get; set; }
}