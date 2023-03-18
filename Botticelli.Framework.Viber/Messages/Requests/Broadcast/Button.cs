using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests.Broadcast;

public class Button
{
    [JsonPropertyName("ActionBody")]
    public string ActionBody { get; set; }

    [JsonPropertyName("ActionType")]
    public string ActionType { get; set; }

    [JsonPropertyName("Text")]
    public string Text { get; set; }
}