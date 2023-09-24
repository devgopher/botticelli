using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class Properties
{
    [JsonPropertyName("response")]
    public Response Response { get; set; }

    [JsonPropertyName("count")]
    public Count Count { get; set; }

    [JsonPropertyName("items")]
    public Items Items { get; set; }

    [JsonPropertyName("profiles")]
    public Profiles Profiles { get; set; }

    [JsonPropertyName("groups")]
    public Groups Groups { get; set; }

    [JsonPropertyName("preview")]
    public Preview Preview { get; set; }

    [JsonPropertyName("unread_count")]
    public UnreadCount UnreadCount { get; set; }

    [JsonPropertyName("next_from")]
    public NextFrom NextFrom { get; set; }

    [JsonPropertyName("message_id")]
    public MessageId MessageId { get; set; }

    [JsonPropertyName("chat")]
    public Chat Chat { get; set; }

    [JsonPropertyName("conversations")]
    public Conversations Conversations { get; set; }

    [JsonPropertyName("messages")]
    public Messages Messages { get; set; }

    [JsonPropertyName("link")]
    public Link Link { get; set; }

    [JsonPropertyName("history")]
    public History History { get; set; }

    [JsonPropertyName("credentials")]
    public Credentials Credentials { get; set; }

    [JsonPropertyName("chats")]
    public Chats Chats { get; set; }

    [JsonPropertyName("new_pts")]
    public NewPts NewPts { get; set; }

    [JsonPropertyName("from_pts")]
    public FromPts FromPts { get; set; }

    [JsonPropertyName("more")]
    public More More { get; set; }

    [JsonPropertyName("is_allowed")]
    public IsAllowed IsAllowed { get; set; }

    [JsonPropertyName("chat_id")]
    public ChatId ChatId { get; set; }

    [JsonPropertyName("last_deleted_id")]
    public LastDeletedId LastDeletedId { get; set; }
}