using System.Text.Json.Serialization;

namespace Viber.Api.Requests
{
    public class RemoveWebHookRequest : BaseRequest
    {
        [JsonPropertyName("url")] public string? Url { get; set; }
    }
}