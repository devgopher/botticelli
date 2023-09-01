using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class Attachment
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("owner_id")]
    public string OwnerId { get; set; }

    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }
    [JsonPropertyName("access_key")]
    public string AccessKey { get; set; }
}