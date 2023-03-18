using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests.Broadcast;

public class RichMedia
{
    [JsonPropertyName("Type")]
    public string Type { get; set; }

    [JsonPropertyName("BgColor")]
    public string BgColor { get; set; }

    [JsonPropertyName("Buttons")]
    public List<Button> Buttons { get; set; }
}