using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests;

public class SendTextMessageRequest
{
    [JsonPropertyName("receiver")]
    public string Receiver { get; set; }

    [JsonPropertyName("min_api_version")]
    public int MinApiVersion { get; set; }

    [JsonPropertyName("sender")]
    public Sender Sender { get; set; }

    [JsonPropertyName("tracking_data")]
    public string TrackingData { get; set; }

    [JsonPropertyName("type")]
    public virtual string Type => "text";

    [JsonPropertyName("text")]
    public string Text { get; set; }
}