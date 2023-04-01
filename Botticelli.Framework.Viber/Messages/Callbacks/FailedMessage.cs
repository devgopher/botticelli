using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Callbacks;

public class FailedMessage : WebHookMessage
{
    [JsonPropertyName("event")]
    public override string Event => EventTypes.Failed;
}