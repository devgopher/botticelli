using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class GetUploadAddress
{
    [JsonPropertyName("response")]
    public GetUploadAddressResponse Response { get; set; }
}