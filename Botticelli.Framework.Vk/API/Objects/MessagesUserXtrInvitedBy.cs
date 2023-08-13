using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class MessagesUserXtrInvitedBy
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("allOf")]
    public List<AllOf> AllOf { get; set; }

    [JsonPropertyName("additionalProperties")]
    public bool AdditionalProperties { get; set; }
}