using System.Text.Json.Serialization;
using Viber.Api.Entities;

namespace Botticelli.Framework.Viber.Messages.Callbacks;

public class WebHookReceivedMessage : WebHookMessage
{
    [JsonPropertyName("event")]
    public override string Event => EventTypes.Message;

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("message_token")]
    public long MessageToken { get; set; }

    [JsonPropertyName("sender")]
    public Sender Sender { get; set; }

    [JsonPropertyName("message")]
    public ViberMessage ViberMessage { get; set; }
}