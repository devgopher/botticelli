using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class Definitions
{
    [JsonPropertyName("messages_createChat_response")]
    public MessagesCreateChatResponse MessagesCreateChatResponse { get; set; }

    [JsonPropertyName("messages_deleteChatPhoto_response")]
    public MessagesDeleteChatPhotoResponse MessagesDeleteChatPhotoResponse { get; set; }

    [JsonPropertyName("messages_deleteConversation_response")]
    public MessagesDeleteConversationResponse MessagesDeleteConversationResponse { get; set; }

    [JsonPropertyName("messages_delete_response")]
    public MessagesDeleteResponse MessagesDeleteResponse { get; set; }

    [JsonPropertyName("messages_edit_response")]
    public MessagesEditResponse MessagesEditResponse { get; set; }

    [JsonPropertyName("messages_getByConversationMessageId_extended_response")]
    public MessagesGetByConversationMessageIdExtendedResponse MessagesGetByConversationMessageIdExtendedResponse { get; set; }

    [JsonPropertyName("messages_getByConversationMessageId_response")]
    public MessagesGetByConversationMessageIdResponse MessagesGetByConversationMessageIdResponse { get; set; }

    [JsonPropertyName("messages_getById_extended_response")]
    public MessagesGetByIdExtendedResponse MessagesGetByIdExtendedResponse { get; set; }

    [JsonPropertyName("messages_getById_response")]
    public MessagesGetByIdResponse MessagesGetByIdResponse { get; set; }

    [JsonPropertyName("messages_getChatPreview_response")]
    public MessagesGetChatPreviewResponse MessagesGetChatPreviewResponse { get; set; }

    [JsonPropertyName("messages_getChat_chat_ids_fields_response")]
    public MessagesGetChatChatIdsFieldsResponse MessagesGetChatChatIdsFieldsResponse { get; set; }

    [JsonPropertyName("messages_getChat_chat_ids_response")]
    public MessagesGetChatChatIdsResponse MessagesGetChatChatIdsResponse { get; set; }

    [JsonPropertyName("messages_getChat_fields_response")]
    public MessagesGetChatFieldsResponse MessagesGetChatFieldsResponse { get; set; }

    [JsonPropertyName("messages_getChat_response")]
    public MessagesGetChatResponse MessagesGetChatResponse { get; set; }

    [JsonPropertyName("messages_getConversationMembers_response")]
    public MessagesGetConversationMembersResponse MessagesGetConversationMembersResponse { get; set; }

    [JsonPropertyName("messages_getConversationsById_extended_response")]
    public MessagesGetConversationsByIdExtendedResponse MessagesGetConversationsByIdExtendedResponse { get; set; }

    [JsonPropertyName("messages_getConversationsById_response")]
    public MessagesGetConversationsByIdResponse MessagesGetConversationsByIdResponse { get; set; }

    [JsonPropertyName("messages_getConversations_response")]
    public MessagesGetConversationsResponse MessagesGetConversationsResponse { get; set; }

    [JsonPropertyName("messages_getHistoryAttachments_response")]
    public MessagesGetHistoryAttachmentsResponse MessagesGetHistoryAttachmentsResponse { get; set; }

    [JsonPropertyName("messages_getHistory_extended_response")]
    public MessagesGetHistoryExtendedResponse MessagesGetHistoryExtendedResponse { get; set; }

    [JsonPropertyName("messages_getHistory_response")]
    public MessagesGetHistoryResponse MessagesGetHistoryResponse { get; set; }

    [JsonPropertyName("messages_getImportantMessages_extended_response")]
    public MessagesGetImportantMessagesExtendedResponse MessagesGetImportantMessagesExtendedResponse { get; set; }

    [JsonPropertyName("messages_getImportantMessages_response")]
    public MessagesGetImportantMessagesResponse MessagesGetImportantMessagesResponse { get; set; }

    [JsonPropertyName("messages_getIntentUsers_response")]
    public MessagesGetIntentUsersResponse MessagesGetIntentUsersResponse { get; set; }

    [JsonPropertyName("messages_getInviteLink_response")]
    public MessagesGetInviteLinkResponse MessagesGetInviteLinkResponse { get; set; }

    [JsonPropertyName("messages_getLastActivity_response")]
    public MessagesGetLastActivityResponse MessagesGetLastActivityResponse { get; set; }

    [JsonPropertyName("messages_getLongPollHistory_response")]
    public MessagesGetLongPollHistoryResponse MessagesGetLongPollHistoryResponse { get; set; }

    [JsonPropertyName("messages_getLongPollServer_response")]
    public MessagesGetLongPollServerResponse MessagesGetLongPollServerResponse { get; set; }

    [JsonPropertyName("messages_isMessagesFromGroupAllowed_response")]
    public MessagesIsMessagesFromGroupAllowedResponse MessagesIsMessagesFromGroupAllowedResponse { get; set; }

    [JsonPropertyName("messages_joinChatByInviteLink_response")]
    public MessagesJoinChatByInviteLinkResponse MessagesJoinChatByInviteLinkResponse { get; set; }

    [JsonPropertyName("messages_markAsImportant_response")]
    public MessagesMarkAsImportantResponse MessagesMarkAsImportantResponse { get; set; }

    [JsonPropertyName("messages_pin_response")]
    public MessagesPinResponse MessagesPinResponse { get; set; }

    [JsonPropertyName("messages_searchConversations_extended_response")]
    public MessagesSearchConversationsExtendedResponse MessagesSearchConversationsExtendedResponse { get; set; }

    [JsonPropertyName("messages_searchConversations_response")]
    public MessagesSearchConversationsResponse MessagesSearchConversationsResponse { get; set; }

    [JsonPropertyName("messages_search_extended_response")]
    public MessagesSearchExtendedResponse MessagesSearchExtendedResponse { get; set; }

    [JsonPropertyName("messages_search_response")]
    public MessagesSearchResponse MessagesSearchResponse { get; set; }

    [JsonPropertyName("messages_send_response")]
    public MessagesSendResponse MessagesSendResponse { get; set; }

    [JsonPropertyName("messages_send_user_ids_response")]
    public MessagesSendUserIdsResponse MessagesSendUserIdsResponse { get; set; }

    [JsonPropertyName("messages_setChatPhoto_response")]
    public MessagesSetChatPhotoResponse MessagesSetChatPhotoResponse { get; set; }
}