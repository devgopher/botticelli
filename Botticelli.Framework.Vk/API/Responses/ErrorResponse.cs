using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Responses;

/// <summary>
///     Update() response
/// </summary>
public class ErrorResponse
{
    /// <summary>
    ///     Offset
    /// </summary>
    [JsonPropertyName("ts")]
    public int? Ts { get; set; }

    /// <summary>
    ///     Error code
    /// </summary>
    [JsonPropertyName("failed")]
    public int Failed { get; set; }

    /// <summary>
    ///     Min API version
    /// </summary>
    [JsonPropertyName("min_version")]
    public int MinVersion { get; set; }

    /// <summary>
    ///     Max API version
    /// </summary>
    [JsonPropertyName("max_version")]
    public int MaxVersion { get; set; }
}