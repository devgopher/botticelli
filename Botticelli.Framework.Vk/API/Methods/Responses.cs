using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Methods;

public class Responses
{
    [JsonPropertyName("response")]
    public Response Response { get; set; }

    [JsonPropertyName("extendedResponse")]
    public ExtendedResponse ExtendedResponse { get; set; }

    [JsonPropertyName("userIdsResponse")]
    public UserIdsResponse UserIdsResponse { get; set; }
}