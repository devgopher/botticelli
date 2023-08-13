using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects;

public class Definitions
{
    [JsonPropertyName("messages_audio_message")]
    public MessagesAudioMessage MessagesAudioMessage { get; set; }

    [JsonPropertyName("messages_chat")]
    public MessagesChat MessagesChat { get; set; }

    [JsonPropertyName("messages_chat_full")]
    public MessagesChatFull MessagesChatFull { get; set; }

    [JsonPropertyName("messages_chat_preview")]
    public MessagesChatPreview MessagesChatPreview { get; set; }

    [JsonPropertyName("messages_chat_push_settings")]
    public MessagesChatPushSettings MessagesChatPushSettings { get; set; }

    [JsonPropertyName("messages_chat_restrictions")]
    public MessagesChatRestrictions MessagesChatRestrictions { get; set; }

    [JsonPropertyName("messages_chat_settings")]
    public MessagesChatSettings MessagesChatSettings { get; set; }

    [JsonPropertyName("messages_chat_settings_acl")]
    public MessagesChatSettingsAcl MessagesChatSettingsAcl { get; set; }

    [JsonPropertyName("messages_chat_settings_permissions")]
    public MessagesChatSettingsPermissions MessagesChatSettingsPermissions { get; set; }

    [JsonPropertyName("messages_chat_settings_photo")]
    public MessagesChatSettingsPhoto MessagesChatSettingsPhoto { get; set; }

    [JsonPropertyName("messages_chat_settings_state")]
    public MessagesChatSettingsState MessagesChatSettingsState { get; set; }

    [JsonPropertyName("messages_conversation")]
    public MessagesConversation MessagesConversation { get; set; }

    [JsonPropertyName("messages_conversation_can_write")]
    public MessagesConversationCanWrite MessagesConversationCanWrite { get; set; }

    [JsonPropertyName("messages_conversation_member")]
    public MessagesConversationMember MessagesConversationMember { get; set; }

    [JsonPropertyName("messages_conversation_peer")]
    public MessagesConversationPeer MessagesConversationPeer { get; set; }

    [JsonPropertyName("messages_conversation_peer_type")]
    public MessagesConversationPeerType MessagesConversationPeerType { get; set; }

    [JsonPropertyName("messages_conversation_sort_id")]
    public MessagesConversationSortId MessagesConversationSortId { get; set; }

    [JsonPropertyName("messages_conversation_with_message")]
    public MessagesConversationWithMessage MessagesConversationWithMessage { get; set; }

    [JsonPropertyName("messages_foreign_message")]
    public MessagesForeignMessage MessagesForeignMessage { get; set; }

    [JsonPropertyName("messages_forward")]
    public MessagesForward MessagesForward { get; set; }

    [JsonPropertyName("messages_getConversationById")]
    public MessagesGetConversationById MessagesGetConversationById { get; set; }

    [JsonPropertyName("messages_getConversationById_extended")]
    public MessagesGetConversationByIdExtended MessagesGetConversationByIdExtended { get; set; }

    [JsonPropertyName("messages_getConversationMembers")]
    public MessagesGetConversationMembers MessagesGetConversationMembers { get; set; }

    [JsonPropertyName("messages_graffiti")]
    public MessagesGraffiti MessagesGraffiti { get; set; }

    [JsonPropertyName("messages_history_attachment")]
    public MessagesHistoryAttachment MessagesHistoryAttachment { get; set; }

    [JsonPropertyName("messages_history_message_attachment")]
    public MessagesHistoryMessageAttachment MessagesHistoryMessageAttachment { get; set; }

    [JsonPropertyName("messages_history_message_attachment_type")]
    public MessagesHistoryMessageAttachmentType MessagesHistoryMessageAttachmentType { get; set; }

    [JsonPropertyName("messages_keyboard")]
    public MessagesKeyboard MessagesKeyboard { get; set; }

    [JsonPropertyName("messages_keyboard_button")]
    public MessagesKeyboardButton MessagesKeyboardButton { get; set; }

    [JsonPropertyName("messages_keyboard_button_action_callback")]
    public MessagesKeyboardButtonActionCallback MessagesKeyboardButtonActionCallback { get; set; }

    [JsonPropertyName("messages_keyboard_button_action_location")]
    public MessagesKeyboardButtonActionLocation MessagesKeyboardButtonActionLocation { get; set; }

    [JsonPropertyName("messages_keyboard_button_action_open_app")]
    public MessagesKeyboardButtonActionOpenApp MessagesKeyboardButtonActionOpenApp { get; set; }

    [JsonPropertyName("messages_keyboard_button_action_open_link")]
    public MessagesKeyboardButtonActionOpenLink MessagesKeyboardButtonActionOpenLink { get; set; }

    [JsonPropertyName("messages_keyboard_button_action_open_photo")]
    public MessagesKeyboardButtonActionOpenPhoto MessagesKeyboardButtonActionOpenPhoto { get; set; }

    [JsonPropertyName("messages_keyboard_button_action_text")]
    public MessagesKeyboardButtonActionText MessagesKeyboardButtonActionText { get; set; }

    [JsonPropertyName("messages_keyboard_button_action_vkpay")]
    public MessagesKeyboardButtonActionVkpay MessagesKeyboardButtonActionVkpay { get; set; }

    [JsonPropertyName("messages_keyboard_button_property_action")]
    public MessagesKeyboardButtonPropertyAction MessagesKeyboardButtonPropertyAction { get; set; }

    [JsonPropertyName("messages_last_activity")]
    public MessagesLastActivity MessagesLastActivity { get; set; }

    [JsonPropertyName("messages_longpoll_messages")]
    public MessagesLongpollMessages MessagesLongpollMessages { get; set; }

    [JsonPropertyName("messages_longpoll_params")]
    public MessagesLongpollParams MessagesLongpollParams { get; set; }

    [JsonPropertyName("messages_message")]
    public MessagesMessage MessagesMessage { get; set; }

    [JsonPropertyName("messages_message_action")]
    public MessagesMessageAction MessagesMessageAction { get; set; }

    [JsonPropertyName("messages_message_action_photo")]
    public MessagesMessageActionPhoto MessagesMessageActionPhoto { get; set; }

    [JsonPropertyName("messages_message_action_status")]
    public MessagesMessageActionStatus MessagesMessageActionStatus { get; set; }

    [JsonPropertyName("messages_message_attachment")]
    public MessagesMessageAttachment MessagesMessageAttachment { get; set; }

    [JsonPropertyName("messages_message_attachment_type")]
    public MessagesMessageAttachmentType MessagesMessageAttachmentType { get; set; }

    [JsonPropertyName("messages_message_request_data")]
    public MessagesMessageRequestData MessagesMessageRequestData { get; set; }

    [JsonPropertyName("messages_messages_array")]
    public MessagesMessagesArray MessagesMessagesArray { get; set; }

    [JsonPropertyName("messages_out_read_by")]
    public MessagesOutReadBy MessagesOutReadBy { get; set; }

    [JsonPropertyName("messages_pinned_message")]
    public MessagesPinnedMessage MessagesPinnedMessage { get; set; }

    [JsonPropertyName("messages_push_settings")]
    public MessagesPushSettings MessagesPushSettings { get; set; }

    [JsonPropertyName("messages_send_user_ids_response_item")]
    public MessagesSendUserIdsResponseItem MessagesSendUserIdsResponseItem { get; set; }

    [JsonPropertyName("messages_template_action_type_names")]
    public MessagesTemplateActionTypeNames MessagesTemplateActionTypeNames { get; set; }

    [JsonPropertyName("messages_user_xtr_invited_by")]
    public MessagesUserXtrInvitedBy MessagesUserXtrInvitedBy { get; set; }
}