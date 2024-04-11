using System.Text.Json.Serialization;

namespace Botticelli.Framework.Facebook.Messages.API.Requests;

public class Recipient
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}