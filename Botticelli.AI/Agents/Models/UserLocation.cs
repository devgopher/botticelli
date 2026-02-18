using System.Text.Json.Serialization;

namespace Botticelli.AI.Agents.Models;

/// <summary>
/// User location context that can be passed to the assistant (e.g. for localized or time-aware answers).
/// </summary>
public class UserLocation
{
    /// <summary>Location type or source.</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>City name.</summary>
    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    /// <summary>Country name or code.</summary>
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    /// <summary>Region or state.</summary>
    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    /// <summary>Timezone identifier (e.g. "Europe/Moscow").</summary>
    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = string.Empty;

    /// <summary>Indicates whether this location was successfully parsed.</summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}