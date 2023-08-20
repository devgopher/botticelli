using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

/// <summary>
/// Update() response
/// </summary>
public class UpdatesResponse
{
    /// <summary>
    /// Offset
    /// </summary>
    [JsonPropertyName("ts")]
    public string Ts { get; set; }

    /// <summary>
    /// Updated objects
    /// </summary>
    [JsonPropertyName("updates")]
    public List<UpdateEvent> Updates { get; set; }
}
