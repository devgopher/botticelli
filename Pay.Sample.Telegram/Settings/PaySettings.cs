using System.Text.Json.Serialization;

namespace TelegramPayBot.Settings;

public class PaySettings
{
    [JsonPropertyName("ProviderToken")]
    public string? ProviderToken { get; init; }
}