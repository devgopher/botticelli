using System.Text.Json.Serialization;
using Viber.Api.Entities;

namespace Botticelli.Framework.Viber.Messages.Callbacks;

public class SubscribedMessage : WebHookMessage
{
    [JsonPropertyName("event")]
    public override string Event => EventTypes.Subscribed;

    [JsonPropertyName("user")]
    public User User { get; set; }
}