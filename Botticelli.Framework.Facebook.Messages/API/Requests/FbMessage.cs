using System.Text.Json.Serialization;

namespace Botticelli.Framework.Facebook.Messages.API.Requests;

public class FbMessage
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}