using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

/// <summary>
/// Particular events
/// </summary>
public class UpdateEvent
{
    /// <summary>
    /// Event type
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// Id of a group
    /// </summary>
    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    /// <summary>
    /// Inner object
    /// </summary>
    [JsonPropertyName("object")]
    public IApiObject Object { get; set; }

}