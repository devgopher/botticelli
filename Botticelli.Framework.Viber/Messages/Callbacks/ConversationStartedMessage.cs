using System.Text.Json.Serialization;
using Botticelli.Framework.Viber.Messages.Callbacks.Models;

namespace Botticelli.Framework.Viber.Messages.Callbacks;

public class ConversationStartedMessage : WebHookMessage
{
    [JsonPropertyName("event")]
    public override string Event => EventTypes.ConversationStarted;

    [JsonPropertyName("message_token")]
    public long MessageToken { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("context")]
    public string Context { get; set; }

    [JsonPropertyName("user")]
    public User User { get; set; }

    [JsonPropertyName("subscribed")]
    public bool Subscribed { get; set; }
}