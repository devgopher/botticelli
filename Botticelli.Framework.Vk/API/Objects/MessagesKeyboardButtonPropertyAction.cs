using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Objects;

public class MessagesKeyboardButtonPropertyAction
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("oneOf")]
    public List<OneOf> OneOf { get; set; }
}