using System.Text.Json.Serialization;
using Botticelli.Framework.Viber.Messages.Callbacks.Models;

namespace Botticelli.Framework.Viber.Messages.Callbacks;

public class SubscribedMessage : WebHookMessage
{
    [JsonPropertyName("event")]
    public override string Event => EventTypes.Subscribed;
    [JsonPropertyName("user")]
    public User User { get; set; }
}