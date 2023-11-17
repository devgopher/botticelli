using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class SessionDataResponse
{
    [JsonPropertyName("server")] public string Server { get; set; }

    [JsonPropertyName("key")] public string Key { get; set; }

    [JsonPropertyName("ts")] public string Ts { get; set; }
}