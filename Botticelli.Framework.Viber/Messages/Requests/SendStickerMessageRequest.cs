using System.Text.Json.Serialization;

namespace Botticelli.Framework.Viber.Messages.Requests;

public class SendStickerMessageRequest : SendMediaRequest
{
    [JsonPropertyName("type")]
    public override string Type => "sticker";

    [JsonPropertyName("sticker_id")]
    public int StickerId { get; set; }
}