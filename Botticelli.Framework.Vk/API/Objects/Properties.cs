using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.Messages.API.Objects;

public class Properties
{
    [JsonPropertyName("access_key")]
    public AccessKey AccessKey { get; set; }

    [JsonPropertyName("transcript_error")]
    public TranscriptError TranscriptError { get; set; }

    [JsonPropertyName("duration")]
    public Duration Duration { get; set; }

    [JsonPropertyName("id")]
    public Id Id { get; set; }

    [JsonPropertyName("link_mp3")]
    public LinkMp3 LinkMp3 { get; set; }

    [JsonPropertyName("link_ogg")]
    public LinkOgg LinkOgg { get; set; }

    [JsonPropertyName("owner_id")]
    public OwnerId OwnerId { get; set; }

    [JsonPropertyName("waveform")]
    public Waveform Waveform { get; set; }

    [JsonPropertyName("photo_50")]
    public Photo50 Photo50 { get; set; }

    [JsonPropertyName("photo_100")]
    public Photo100 Photo100 { get; set; }

    [JsonPropertyName("photo_200")]
    public Photo200 Photo200 { get; set; }

    [JsonPropertyName("is_default_photo")]
    public IsDefaultPhoto IsDefaultPhoto { get; set; }

    [JsonPropertyName("is_default_call_photo")]
    public IsDefaultCallPhoto IsDefaultCallPhoto { get; set; }

    [JsonPropertyName("peer")]
    public Peer Peer { get; set; }

    [JsonPropertyName("sort_id")]
    public SortId SortId { get; set; }

    [JsonPropertyName("last_message_id")]
    public LastMessageId LastMessageId { get; set; }

    [JsonPropertyName("last_conversation_message_id")]
    public LastConversationMessageId LastConversationMessageId { get; set; }

    [JsonPropertyName("in_read")]
    public InRead InRead { get; set; }

    [JsonPropertyName("out_read")]
    public OutRead OutRead { get; set; }

    [JsonPropertyName("unread_count")]
    public UnreadCount UnreadCount { get; set; }

    [JsonPropertyName("is_marked_unread")]
    public IsMarkedUnread IsMarkedUnread { get; set; }

    [JsonPropertyName("out_read_by")]
    public OutReadBy OutReadBy { get; set; }

    [JsonPropertyName("important")]
    public Important Important { get; set; }

    [JsonPropertyName("unanswered")]
    public Unanswered Unanswered { get; set; }

    [JsonPropertyName("special_service_type")]
    public SpecialServiceType SpecialServiceType { get; set; }

    [JsonPropertyName("message_request_data")]
    public MessageRequestData MessageRequestData { get; set; }

    [JsonPropertyName("mentions")]
    public Mentions Mentions { get; set; }

    [JsonPropertyName("current_keyboard")]
    public CurrentKeyboard CurrentKeyboard { get; set; }

    [JsonPropertyName("push_settings")]
    public PushSettings PushSettings { get; set; }

    [JsonPropertyName("can_write")]
    public CanWrite CanWrite { get; set; }

    [JsonPropertyName("chat_settings")]
    public ChatSettings ChatSettings { get; set; }

    [JsonPropertyName("allowed")]
    public Allowed Allowed { get; set; }

    [JsonPropertyName("reason")]
    public Reason Reason { get; set; }

    [JsonPropertyName("can_kick")]
    public CanKick CanKick { get; set; }

    [JsonPropertyName("invited_by")]
    public InvitedBy InvitedBy { get; set; }

    [JsonPropertyName("is_admin")]
    public IsAdmin IsAdmin { get; set; }

    [JsonPropertyName("is_owner")]
    public IsOwner IsOwner { get; set; }

    [JsonPropertyName("is_message_request")]
    public IsMessageRequest IsMessageRequest { get; set; }

    [JsonPropertyName("join_date")]
    public JoinDate JoinDate { get; set; }

    [JsonPropertyName("request_date")]
    public RequestDate RequestDate { get; set; }

    [JsonPropertyName("member_id")]
    public MemberId MemberId { get; set; }

    [JsonPropertyName("local_id")]
    public LocalId LocalId { get; set; }

    [JsonPropertyName("type")]
    public Type Type { get; set; }

    [JsonPropertyName("major_id")]
    public MajorId MajorId { get; set; }

    [JsonPropertyName("minor_id")]
    public MinorId MinorId { get; set; }

    [JsonPropertyName("conversation")]
    public Conversation Conversation { get; set; }

    [JsonPropertyName("last_message")]
    public LastMessage LastMessage { get; set; }

    [JsonPropertyName("attachments")]
    public Attachments Attachments { get; set; }

    [JsonPropertyName("conversation_message_id")]
    public ConversationMessageId ConversationMessageId { get; set; }

    [JsonPropertyName("date")]
    public Date Date { get; set; }

    [JsonPropertyName("from_id")]
    public FromId FromId { get; set; }

    [JsonPropertyName("fwd_messages")]
    public FwdMessages FwdMessages { get; set; }

    [JsonPropertyName("geo")]
    public Geo Geo { get; set; }

    [JsonPropertyName("peer_id")]
    public PeerId PeerId { get; set; }

    [JsonPropertyName("reply_message")]
    public ReplyMessage ReplyMessage { get; set; }

    [JsonPropertyName("text")]
    public Text Text { get; set; }

    [JsonPropertyName("update_time")]
    public UpdateTime UpdateTime { get; set; }

    [JsonPropertyName("was_listened")]
    public WasListened WasListened { get; set; }

    [JsonPropertyName("payload")]
    public Payload Payload { get; set; }

    [JsonPropertyName("conversation_message_ids")]
    public ConversationMessageIds ConversationMessageIds { get; set; }

    [JsonPropertyName("message_ids")]
    public MessageIds MessageIds { get; set; }

    [JsonPropertyName("is_reply")]
    public IsReply IsReply { get; set; }

    [JsonPropertyName("count")]
    public Count Count { get; set; }

    [JsonPropertyName("items")]
    public Items Items { get; set; }

    [JsonPropertyName("admin_id")]
    public AdminId AdminId { get; set; }

    [JsonPropertyName("kicked")]
    public Kicked Kicked { get; set; }

    [JsonPropertyName("left")]
    public Left Left { get; set; }

    [JsonPropertyName("title")]
    public Title Title { get; set; }

    [JsonPropertyName("users")]
    public Users Users { get; set; }

    [JsonPropertyName("members_count")]
    public MembersCount MembersCount { get; set; }

    [JsonPropertyName("is_group_channel")]
    public IsGroupChannel IsGroupChannel { get; set; }

    [JsonPropertyName("profiles")]
    public Profiles Profiles { get; set; }

    [JsonPropertyName("groups")]
    public Groups Groups { get; set; }

    [JsonPropertyName("chat_restrictions")]
    public ChatRestrictions ChatRestrictions { get; set; }

    [JsonPropertyName("url")]
    public Url Url { get; set; }

    [JsonPropertyName("width")]
    public Width Width { get; set; }

    [JsonPropertyName("height")]
    public Height Height { get; set; }

    [JsonPropertyName("attachment")]
    public Attachment Attachment { get; set; }

    [JsonPropertyName("message_id")]
    public MessageId MessageId { get; set; }

    [JsonPropertyName("forward_level")]
    public ForwardLevel ForwardLevel { get; set; }

    [JsonPropertyName("audio")]
    public Audio Audio { get; set; }

    [JsonPropertyName("audio_message")]
    public AudioMessage AudioMessage { get; set; }

    [JsonPropertyName("doc")]
    public Doc Doc { get; set; }

    [JsonPropertyName("graffiti")]
    public Graffiti Graffiti { get; set; }

    [JsonPropertyName("link")]
    public Link Link { get; set; }

    [JsonPropertyName("market")]
    public Market Market { get; set; }

    [JsonPropertyName("photo")]
    public Photo Photo { get; set; }

    [JsonPropertyName("video")]
    public Video Video { get; set; }

    [JsonPropertyName("wall")]
    public Wall Wall { get; set; }

    [JsonPropertyName("one_time")]
    public OneTime OneTime { get; set; }

    [JsonPropertyName("buttons")]
    public Buttons Buttons { get; set; }

    [JsonPropertyName("author_id")]
    public AuthorId AuthorId { get; set; }

    [JsonPropertyName("inline")]
    public Inline Inline { get; set; }

    [JsonPropertyName("action")]
    public Action Action { get; set; }

    [JsonPropertyName("color")]
    public Color Color { get; set; }

    [JsonPropertyName("label")]
    public Label Label { get; set; }

    [JsonPropertyName("app_id")]
    public AppId AppId { get; set; }

    [JsonPropertyName("hash")]
    public Hash Hash { get; set; }

    [JsonPropertyName("online")]
    public Online Online { get; set; }

    [JsonPropertyName("time")]
    public Time Time { get; set; }

    [JsonPropertyName("server")]
    public Server Server { get; set; }

    [JsonPropertyName("key")]
    public Key Key { get; set; }

    [JsonPropertyName("ts")]
    public Ts Ts { get; set; }

    [JsonPropertyName("pts")]
    public Pts Pts { get; set; }

    [JsonPropertyName("admin_author_id")]
    public AdminAuthorId AdminAuthorId { get; set; }

    [JsonPropertyName("deleted")]
    public Deleted Deleted { get; set; }

    [JsonPropertyName("is_hidden")]
    public IsHidden IsHidden { get; set; }

    [JsonPropertyName("is_cropped")]
    public IsCropped IsCropped { get; set; }

    [JsonPropertyName("keyboard")]
    public Keyboard Keyboard { get; set; }

    [JsonPropertyName("out")]
    public Out Out { get; set; }

    [JsonPropertyName("random_id")]
    public RandomId RandomId { get; set; }

    [JsonPropertyName("ref")]
    public Ref Ref { get; set; }

    [JsonPropertyName("ref_source")]
    public RefSource RefSource { get; set; }

    [JsonPropertyName("pinned_at")]
    public PinnedAt PinnedAt { get; set; }

    [JsonPropertyName("is_silent")]
    public IsSilent IsSilent { get; set; }

    [JsonPropertyName("email")]
    public Email Email { get; set; }

    [JsonPropertyName("message")]
    public Message Message { get; set; }

    [JsonPropertyName("joined")]
    public Joined Joined { get; set; }

    [JsonPropertyName("members")]
    public Members Members { get; set; }

    [JsonPropertyName("is_member")]
    public IsMember IsMember { get; set; }

    [JsonPropertyName("is_don")]
    public IsDon IsDon { get; set; }

    [JsonPropertyName("button")]
    public Button Button { get; set; }

    [JsonPropertyName("call")]
    public Call Call { get; set; }

    [JsonPropertyName("gift")]
    public Gift Gift { get; set; }

    [JsonPropertyName("market_market_album")]
    public MarketMarketAlbum MarketMarketAlbum { get; set; }

    [JsonPropertyName("sticker")]
    public Sticker Sticker { get; set; }

    [JsonPropertyName("story")]
    public Story Story { get; set; }

    [JsonPropertyName("wall_reply")]
    public WallReply WallReply { get; set; }

    [JsonPropertyName("poll")]
    public Poll Poll { get; set; }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("inviter_id")]
    public InviterId InviterId { get; set; }

    [JsonPropertyName("member_ids")]
    public MemberIds MemberIds { get; set; }

    [JsonPropertyName("disabled_forever")]
    public DisabledForever DisabledForever { get; set; }

    [JsonPropertyName("disabled_until")]
    public DisabledUntil DisabledUntil { get; set; }

    [JsonPropertyName("no_sound")]
    public NoSound NoSound { get; set; }

    [JsonPropertyName("disabled_mentions")]
    public DisabledMentions DisabledMentions { get; set; }

    [JsonPropertyName("disabled_mass_mentions")]
    public DisabledMassMentions DisabledMassMentions { get; set; }

    [JsonPropertyName("error")]
    public Error Error { get; set; }

    [JsonPropertyName("sound")]
    public Sound Sound { get; set; }

    [JsonPropertyName("admins_promote_users")]
    public AdminsPromoteUsers AdminsPromoteUsers { get; set; }

    [JsonPropertyName("only_admins_edit_info")]
    public OnlyAdminsEditInfo OnlyAdminsEditInfo { get; set; }

    [JsonPropertyName("only_admins_edit_pin")]
    public OnlyAdminsEditPin OnlyAdminsEditPin { get; set; }

    [JsonPropertyName("only_admins_invite")]
    public OnlyAdminsInvite OnlyAdminsInvite { get; set; }

    [JsonPropertyName("only_admins_kick")]
    public OnlyAdminsKick OnlyAdminsKick { get; set; }

    [JsonPropertyName("friends_count")]
    public FriendsCount FriendsCount { get; set; }

    [JsonPropertyName("pinned_message")]
    public PinnedMessage PinnedMessage { get; set; }

    [JsonPropertyName("state")]
    public State State { get; set; }

    [JsonPropertyName("admin_ids")]
    public AdminIds AdminIds { get; set; }

    [JsonPropertyName("active_ids")]
    public ActiveIds ActiveIds { get; set; }

    [JsonPropertyName("acl")]
    public Acl Acl { get; set; }

    [JsonPropertyName("permissions")]
    public Permissions Permissions { get; set; }

    [JsonPropertyName("is_disappearing")]
    public IsDisappearing IsDisappearing { get; set; }

    [JsonPropertyName("theme")]
    public Theme Theme { get; set; }

    [JsonPropertyName("disappearing_chat_link")]
    public DisappearingChatLink DisappearingChatLink { get; set; }

    [JsonPropertyName("is_service")]
    public IsService IsService { get; set; }

    [JsonPropertyName("can_change_info")]
    public CanChangeInfo CanChangeInfo { get; set; }

    [JsonPropertyName("can_change_invite_link")]
    public CanChangeInviteLink CanChangeInviteLink { get; set; }

    [JsonPropertyName("can_change_pin")]
    public CanChangePin CanChangePin { get; set; }

    [JsonPropertyName("can_invite")]
    public CanInvite CanInvite { get; set; }

    [JsonPropertyName("can_promote_users")]
    public CanPromoteUsers CanPromoteUsers { get; set; }

    [JsonPropertyName("can_see_invite_link")]
    public CanSeeInviteLink CanSeeInviteLink { get; set; }

    [JsonPropertyName("can_moderate")]
    public CanModerate CanModerate { get; set; }

    [JsonPropertyName("can_copy_chat")]
    public CanCopyChat CanCopyChat { get; set; }

    [JsonPropertyName("can_call")]
    public CanCall CanCall { get; set; }

    [JsonPropertyName("can_use_mass_mentions")]
    public CanUseMassMentions CanUseMassMentions { get; set; }

    [JsonPropertyName("can_change_service_type")]
    public CanChangeServiceType CanChangeServiceType { get; set; }

    [JsonPropertyName("invite")]
    public Invite Invite { get; set; }

    [JsonPropertyName("change_info")]
    public ChangeInfo ChangeInfo { get; set; }

    [JsonPropertyName("change_pin")]
    public ChangePin ChangePin { get; set; }

    [JsonPropertyName("use_mass_mentions")]
    public UseMassMentions UseMassMentions { get; set; }

    [JsonPropertyName("see_invite_link")]
    public SeeInviteLink SeeInviteLink { get; set; }

    [JsonPropertyName("change_admins")]
    public ChangeAdmins ChangeAdmins { get; set; }
}