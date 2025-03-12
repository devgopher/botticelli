using System.Text.Json.Serialization;

namespace Viber.Api.Requests
{
    public abstract class BaseRequest
    {
        [JsonPropertyName("auth_token")]
        public string? AuthToken { get; set; }
    }
}