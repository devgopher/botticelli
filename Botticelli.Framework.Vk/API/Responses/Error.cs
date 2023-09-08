using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

public class Error
{
    [JsonPropertyName("error_code")]
    public int ErrorCode { get; set; }

    [JsonPropertyName("error_msg")]
    public string ErrorMsg { get; set; }

    [JsonPropertyName("request_params")]
    public List<object> RequestParams { get; set; }
}