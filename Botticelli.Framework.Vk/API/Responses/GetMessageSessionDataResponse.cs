using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class GetMessageSessionDataResponse
{
    [JsonPropertyName("response")]
    public SessionDataResponse Response { get; set; }
}