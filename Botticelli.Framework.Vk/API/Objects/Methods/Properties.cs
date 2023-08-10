using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods;

public class Properties
{
    [JsonPropertyName("additional_address")]
    public AdditionalAddress AdditionalAddress { get; set; }

    [JsonPropertyName("address")]
    public Address Address { get; set; }

    [JsonPropertyName("city_id")]
    public CityId CityId { get; set; }

    [JsonPropertyName("country_id")]
    public CountryId CountryId { get; set; }

    [JsonPropertyName("distance")]
    public Distance Distance { get; set; }

    [JsonPropertyName("id")]
    public Id Id { get; set; }

    [JsonPropertyName("latitude")]
    public Latitude Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public Longitude Longitude { get; set; }

    [JsonPropertyName("metro_station_id")]
    public MetroStationId MetroStationId { get; set; }

    [JsonPropertyName("phone")]
    public Phone Phone { get; set; }

    [JsonPropertyName("time_offset")]
    public TimeOffset TimeOffset { get; set; }

    [JsonPropertyName("timetable")]
    public Timetable Timetable { get; set; }

    [JsonPropertyName("title")]
    public Title Title { get; set; }

    [JsonPropertyName("work_info_status")]
    public WorkInfoStatus WorkInfoStatus { get; set; }

    [JsonPropertyName("place_id")]
    public PlaceId PlaceId { get; set; }

    [JsonPropertyName("enabled")]
    public Enabled Enabled { get; set; }

    [JsonPropertyName("images")]
    public Images Images { get; set; }

    [JsonPropertyName("name")]
    public Name Name { get; set; }

    [JsonPropertyName("screen_name")]
    public ScreenName ScreenName { get; set; }

    [JsonPropertyName("is_closed")]
    public IsClosed IsClosed { get; set; }

    [JsonPropertyName("type")]
    public Type Type { get; set; }

    [JsonPropertyName("is_admin")]
    public IsAdmin IsAdmin { get; set; }

    [JsonPropertyName("admin_level")]
    public AdminLevel AdminLevel { get; set; }

    [JsonPropertyName("is_member")]
    public IsMember IsMember { get; set; }

    [JsonPropertyName("is_advertiser")]
    public IsAdvertiser IsAdvertiser { get; set; }

    [JsonPropertyName("start_date")]
    public StartDate StartDate { get; set; }

    [JsonPropertyName("finish_date")]
    public FinishDate FinishDate { get; set; }

    [JsonPropertyName("deactivated")]
    public Deactivated Deactivated { get; set; }

    [JsonPropertyName("photo_50")]
    public Photo50 Photo50 { get; set; }

    [JsonPropertyName("photo_100")]
    public Photo100 Photo100 { get; set; }

    [JsonPropertyName("photo_200")]
    public Photo200 Photo200 { get; set; }

    [JsonPropertyName("photo_200_orig")]
    public Photo200Orig Photo200Orig { get; set; }

    [JsonPropertyName("photo_400")]
    public Photo400 Photo400 { get; set; }

    [JsonPropertyName("photo_400_orig")]
    public Photo400Orig Photo400Orig { get; set; }

    [JsonPropertyName("photo_max")]
    public PhotoMax PhotoMax { get; set; }

    [JsonPropertyName("photo_max_orig")]
    public PhotoMaxOrig PhotoMaxOrig { get; set; }

    [JsonPropertyName("est_date")]
    public EstDate EstDate { get; set; }

    [JsonPropertyName("public_date_label")]
    public PublicDateLabel PublicDateLabel { get; set; }

    [JsonPropertyName("photo_max_size")]
    public PhotoMaxSize PhotoMaxSize { get; set; }

    [JsonPropertyName("is_video_live_notifications_blocked")]
    public IsVideoLiveNotificationsBlocked IsVideoLiveNotificationsBlocked { get; set; }

    [JsonPropertyName("video_live")]
    public VideoLive VideoLive { get; set; }

    [JsonPropertyName("text")]
    public Text Text { get; set; }

    [JsonPropertyName("status")]
    public Status Status { get; set; }

    [JsonPropertyName("size")]
    public Size Size { get; set; }

    [JsonPropertyName("is_favorite")]
    public IsFavorite IsFavorite { get; set; }

    [JsonPropertyName("comment")]
    public Comment Comment { get; set; }

    [JsonPropertyName("end_date")]
    public EndDate EndDate { get; set; }

    [JsonPropertyName("reason")]
    public Reason Reason { get; set; }

    [JsonPropertyName("subcategories")]
    public Subcategories Subcategories { get; set; }

    [JsonPropertyName("page_count")]
    public PageCount PageCount { get; set; }

    [JsonPropertyName("page_previews")]
    public PagePreviews PagePreviews { get; set; }

    [JsonPropertyName("market")]
    public Market Market { get; set; }

    [JsonPropertyName("member_status")]
    public MemberStatus MemberStatus { get; set; }

    [JsonPropertyName("is_adult")]
    public IsAdult IsAdult { get; set; }

    [JsonPropertyName("is_hidden_from_feed")]
    public IsHiddenFromFeed IsHiddenFromFeed { get; set; }

    [JsonPropertyName("is_subscribed")]
    public IsSubscribed IsSubscribed { get; set; }

    [JsonPropertyName("city")]
    public City City { get; set; }

    [JsonPropertyName("country")]
    public Country Country { get; set; }

    [JsonPropertyName("verified")]
    public Verified Verified { get; set; }

    [JsonPropertyName("description")]
    public Description Description { get; set; }

    [JsonPropertyName("wiki_page")]
    public WikiPage WikiPage { get; set; }

    [JsonPropertyName("members_count")]
    public MembersCount MembersCount { get; set; }

    [JsonPropertyName("members_count_text")]
    public MembersCountText MembersCountText { get; set; }

    [JsonPropertyName("requests_count")]
    public RequestsCount RequestsCount { get; set; }

    [JsonPropertyName("video_live_level")]
    public VideoLiveLevel VideoLiveLevel { get; set; }

    [JsonPropertyName("video_live_count")]
    public VideoLiveCount VideoLiveCount { get; set; }

    [JsonPropertyName("clips_count")]
    public ClipsCount ClipsCount { get; set; }

    [JsonPropertyName("counters")]
    public Counters Counters { get; set; }

    [JsonPropertyName("cover")]
    public Cover Cover { get; set; }

    [JsonPropertyName("can_post")]
    public CanPost CanPost { get; set; }

    [JsonPropertyName("can_suggest")]
    public CanSuggest CanSuggest { get; set; }

    [JsonPropertyName("can_upload_story")]
    public CanUploadStory CanUploadStory { get; set; }

    [JsonPropertyName("can_upload_doc")]
    public CanUploadDoc CanUploadDoc { get; set; }

    [JsonPropertyName("can_upload_video")]
    public CanUploadVideo CanUploadVideo { get; set; }

    [JsonPropertyName("can_see_all_posts")]
    public CanSeeAllPosts CanSeeAllPosts { get; set; }

    [JsonPropertyName("can_create_topic")]
    public CanCreateTopic CanCreateTopic { get; set; }

    [JsonPropertyName("activity")]
    public Activity Activity { get; set; }

    [JsonPropertyName("fixed_post")]
    public FixedPost FixedPost { get; set; }

    [JsonPropertyName("has_photo")]
    public HasPhoto HasPhoto { get; set; }

    [JsonPropertyName("crop_photo")]
    public CropPhoto CropPhoto { get; set; }

    [JsonPropertyName("status_audio")]
    public StatusAudio StatusAudio { get; set; }

    [JsonPropertyName("main_album_id")]
    public MainAlbumId MainAlbumId { get; set; }

    [JsonPropertyName("links")]
    public Links Links { get; set; }

    [JsonPropertyName("contacts")]
    public Contacts Contacts { get; set; }

    [JsonPropertyName("wall")]
    public Wall Wall { get; set; }

    [JsonPropertyName("site")]
    public Site Site { get; set; }

    [JsonPropertyName("main_section")]
    public MainSection MainSection { get; set; }

    [JsonPropertyName("secondary_section")]
    public SecondarySection SecondarySection { get; set; }

    [JsonPropertyName("trending")]
    public Trending Trending { get; set; }

    [JsonPropertyName("can_message")]
    public CanMessage CanMessage { get; set; }

    [JsonPropertyName("is_messages_blocked")]
    public IsMessagesBlocked IsMessagesBlocked { get; set; }

    [JsonPropertyName("can_send_notify")]
    public CanSendNotify CanSendNotify { get; set; }

    [JsonPropertyName("online_status")]
    public OnlineStatus OnlineStatus { get; set; }

    [JsonPropertyName("invited_by")]
    public InvitedBy InvitedBy { get; set; }

    [JsonPropertyName("age_limits")]
    public AgeLimits AgeLimits { get; set; }

    [JsonPropertyName("ban_info")]
    public BanInfo BanInfo { get; set; }

    [JsonPropertyName("has_market_app")]
    public HasMarketApp HasMarketApp { get; set; }

    [JsonPropertyName("using_vkpay_market_app")]
    public UsingVkpayMarketApp UsingVkpayMarketApp { get; set; }

    [JsonPropertyName("has_group_channel")]
    public HasGroupChannel HasGroupChannel { get; set; }

    [JsonPropertyName("addresses")]
    public Addresses Addresses { get; set; }

    [JsonPropertyName("is_subscribed_podcasts")]
    public IsSubscribedPodcasts IsSubscribedPodcasts { get; set; }

    [JsonPropertyName("can_subscribe_podcasts")]
    public CanSubscribePodcasts CanSubscribePodcasts { get; set; }

    [JsonPropertyName("can_subscribe_posts")]
    public CanSubscribePosts CanSubscribePosts { get; set; }

    [JsonPropertyName("live_covers")]
    public LiveCovers LiveCovers { get; set; }

    [JsonPropertyName("stories_archive_count")]
    public StoriesArchiveCount StoriesArchiveCount { get; set; }

    [JsonPropertyName("has_unseen_stories")]
    public HasUnseenStories HasUnseenStories { get; set; }

    [JsonPropertyName("color")]
    public Color Color { get; set; }

    [JsonPropertyName("uses")]
    public Uses Uses { get; set; }

    [JsonPropertyName("fri")]
    public Fri Fri { get; set; }

    [JsonPropertyName("mon")]
    public Mon Mon { get; set; }

    [JsonPropertyName("sat")]
    public Sat Sat { get; set; }

    [JsonPropertyName("sun")]
    public Sun Sun { get; set; }

    [JsonPropertyName("thu")]
    public Thu Thu { get; set; }

    [JsonPropertyName("tue")]
    public Tue Tue { get; set; }

    [JsonPropertyName("wed")]
    public Wed Wed { get; set; }

    [JsonPropertyName("count")]
    public Count Count { get; set; }

    [JsonPropertyName("items")]
    public Items Items { get; set; }

    [JsonPropertyName("desc")]
    public Desc Desc { get; set; }

    [JsonPropertyName("edit_title")]
    public EditTitle EditTitle { get; set; }

    [JsonPropertyName("url")]
    public Url Url { get; set; }

    [JsonPropertyName("image_processing")]
    public ImageProcessing ImageProcessing { get; set; }

    [JsonPropertyName("is_enabled")]
    public IsEnabled IsEnabled { get; set; }

    [JsonPropertyName("is_scalable")]
    public IsScalable IsScalable { get; set; }

    [JsonPropertyName("story_ids")]
    public StoryIds StoryIds { get; set; }

    [JsonPropertyName("audio_new")]
    public AudioNew AudioNew { get; set; }

    [JsonPropertyName("board_post_delete")]
    public BoardPostDelete BoardPostDelete { get; set; }

    [JsonPropertyName("board_post_edit")]
    public BoardPostEdit BoardPostEdit { get; set; }

    [JsonPropertyName("board_post_new")]
    public BoardPostNew BoardPostNew { get; set; }

    [JsonPropertyName("board_post_restore")]
    public BoardPostRestore BoardPostRestore { get; set; }

    [JsonPropertyName("group_change_photo")]
    public GroupChangePhoto GroupChangePhoto { get; set; }

    [JsonPropertyName("group_change_settings")]
    public GroupChangeSettings GroupChangeSettings { get; set; }

    [JsonPropertyName("group_join")]
    public GroupJoin GroupJoin { get; set; }

    [JsonPropertyName("group_leave")]
    public GroupLeave GroupLeave { get; set; }

    [JsonPropertyName("group_officers_edit")]
    public GroupOfficersEdit GroupOfficersEdit { get; set; }

    [JsonPropertyName("lead_forms_new")]
    public LeadFormsNew LeadFormsNew { get; set; }

    [JsonPropertyName("market_comment_delete")]
    public MarketCommentDelete MarketCommentDelete { get; set; }

    [JsonPropertyName("market_comment_edit")]
    public MarketCommentEdit MarketCommentEdit { get; set; }

    [JsonPropertyName("market_comment_new")]
    public MarketCommentNew MarketCommentNew { get; set; }

    [JsonPropertyName("market_comment_restore")]
    public MarketCommentRestore MarketCommentRestore { get; set; }

    [JsonPropertyName("market_order_new")]
    public MarketOrderNew MarketOrderNew { get; set; }

    [JsonPropertyName("market_order_edit")]
    public MarketOrderEdit MarketOrderEdit { get; set; }

    [JsonPropertyName("message_allow")]
    public MessageAllow MessageAllow { get; set; }

    [JsonPropertyName("message_deny")]
    public MessageDeny MessageDeny { get; set; }

    [JsonPropertyName("message_new")]
    public MessageNew MessageNew { get; set; }

    [JsonPropertyName("message_read")]
    public MessageRead MessageRead { get; set; }

    [JsonPropertyName("message_reply")]
    public MessageReply MessageReply { get; set; }

    [JsonPropertyName("message_typing_state")]
    public MessageTypingState MessageTypingState { get; set; }

    [JsonPropertyName("message_edit")]
    public MessageEdit MessageEdit { get; set; }

    [JsonPropertyName("photo_comment_delete")]
    public PhotoCommentDelete PhotoCommentDelete { get; set; }

    [JsonPropertyName("photo_comment_edit")]
    public PhotoCommentEdit PhotoCommentEdit { get; set; }

    [JsonPropertyName("photo_comment_new")]
    public PhotoCommentNew PhotoCommentNew { get; set; }

    [JsonPropertyName("photo_comment_restore")]
    public PhotoCommentRestore PhotoCommentRestore { get; set; }

    [JsonPropertyName("photo_new")]
    public PhotoNew PhotoNew { get; set; }

    [JsonPropertyName("poll_vote_new")]
    public PollVoteNew PollVoteNew { get; set; }

    [JsonPropertyName("user_block")]
    public UserBlock UserBlock { get; set; }

    [JsonPropertyName("user_unblock")]
    public UserUnblock UserUnblock { get; set; }

    [JsonPropertyName("video_comment_delete")]
    public VideoCommentDelete VideoCommentDelete { get; set; }

    [JsonPropertyName("video_comment_edit")]
    public VideoCommentEdit VideoCommentEdit { get; set; }

    [JsonPropertyName("video_comment_new")]
    public VideoCommentNew VideoCommentNew { get; set; }

    [JsonPropertyName("video_comment_restore")]
    public VideoCommentRestore VideoCommentRestore { get; set; }

    [JsonPropertyName("video_new")]
    public VideoNew VideoNew { get; set; }

    [JsonPropertyName("wall_post_new")]
    public WallPostNew WallPostNew { get; set; }

    [JsonPropertyName("wall_reply_delete")]
    public WallReplyDelete WallReplyDelete { get; set; }

    [JsonPropertyName("wall_reply_edit")]
    public WallReplyEdit WallReplyEdit { get; set; }

    [JsonPropertyName("wall_reply_new")]
    public WallReplyNew WallReplyNew { get; set; }

    [JsonPropertyName("wall_reply_restore")]
    public WallReplyRestore WallReplyRestore { get; set; }

    [JsonPropertyName("wall_repost")]
    public WallRepost WallRepost { get; set; }

    [JsonPropertyName("donut_subscription_create")]
    public DonutSubscriptionCreate DonutSubscriptionCreate { get; set; }

    [JsonPropertyName("donut_subscription_prolonged")]
    public DonutSubscriptionProlonged DonutSubscriptionProlonged { get; set; }

    [JsonPropertyName("donut_subscription_cancelled")]
    public DonutSubscriptionCancelled DonutSubscriptionCancelled { get; set; }

    [JsonPropertyName("donut_subscription_expired")]
    public DonutSubscriptionExpired DonutSubscriptionExpired { get; set; }

    [JsonPropertyName("donut_subscription_price_changed")]
    public DonutSubscriptionPriceChanged DonutSubscriptionPriceChanged { get; set; }

    [JsonPropertyName("donut_money_withdraw")]
    public DonutMoneyWithdraw DonutMoneyWithdraw { get; set; }

    [JsonPropertyName("donut_money_withdraw_error")]
    public DonutMoneyWithdrawError DonutMoneyWithdrawError { get; set; }

    [JsonPropertyName("key")]
    public Key Key { get; set; }

    [JsonPropertyName("server")]
    public Server Server { get; set; }

    [JsonPropertyName("ts")]
    public Ts Ts { get; set; }

    [JsonPropertyName("api_version")]
    public ApiVersion ApiVersion { get; set; }

    [JsonPropertyName("events")]
    public Events Events { get; set; }

    [JsonPropertyName("contact_id")]
    public ContactId ContactId { get; set; }

    [JsonPropertyName("currency")]
    public Currency Currency { get; set; }

    [JsonPropertyName("currency_text")]
    public CurrencyText CurrencyText { get; set; }

    [JsonPropertyName("price_max")]
    public PriceMax PriceMax { get; set; }

    [JsonPropertyName("price_min")]
    public PriceMin PriceMin { get; set; }

    [JsonPropertyName("min_order_price")]
    public MinOrderPrice MinOrderPrice { get; set; }

    [JsonPropertyName("permissions")]
    public Permissions Permissions { get; set; }

    [JsonPropertyName("role")]
    public Role Role { get; set; }

    [JsonPropertyName("member")]
    public Member Member { get; set; }

    [JsonPropertyName("user_id")]
    public UserId UserId { get; set; }

    [JsonPropertyName("can_invite")]
    public CanInvite CanInvite { get; set; }

    [JsonPropertyName("can_recall")]
    public CanRecall CanRecall { get; set; }

    [JsonPropertyName("invitation")]
    public Invitation Invitation { get; set; }

    [JsonPropertyName("request")]
    public Request Request { get; set; }

    [JsonPropertyName("break_close_time")]
    public BreakCloseTime BreakCloseTime { get; set; }

    [JsonPropertyName("break_open_time")]
    public BreakOpenTime BreakOpenTime { get; set; }

    [JsonPropertyName("close_time")]
    public CloseTime CloseTime { get; set; }

    [JsonPropertyName("open_time")]
    public OpenTime OpenTime { get; set; }

    [JsonPropertyName("minutes")]
    public Minutes Minutes { get; set; }

    [JsonPropertyName("group")]
    public Group Group { get; set; }

    [JsonPropertyName("profile")]
    public Profile Profile { get; set; }

    [JsonPropertyName("height")]
    public Height Height { get; set; }

    [JsonPropertyName("width")]
    public Width Width { get; set; }

    [JsonPropertyName("setting")]
    public Setting Setting { get; set; }

    [JsonPropertyName("main_address_id")]
    public MainAddressId MainAddressId { get; set; }

    [JsonPropertyName("admin_id")]
    public AdminId AdminId { get; set; }

    [JsonPropertyName("comment_visible")]
    public CommentVisible CommentVisible { get; set; }

    [JsonPropertyName("date")]
    public Date Date { get; set; }

    [JsonPropertyName("creator_id")]
    public CreatorId CreatorId { get; set; }

    [JsonPropertyName("secret_key")]
    public SecretKey SecretKey { get; set; }

    [JsonPropertyName("email")]
    public Email Email { get; set; }

    [JsonPropertyName("albums")]
    public Albums Albums { get; set; }

    [JsonPropertyName("audios")]
    public Audios Audios { get; set; }

    [JsonPropertyName("audio_playlists")]
    public AudioPlaylists AudioPlaylists { get; set; }

    [JsonPropertyName("docs")]
    public Docs Docs { get; set; }

    [JsonPropertyName("photos")]
    public Photos Photos { get; set; }

    [JsonPropertyName("topics")]
    public Topics Topics { get; set; }

    [JsonPropertyName("videos")]
    public Videos Videos { get; set; }

    [JsonPropertyName("market_services")]
    public MarketServices MarketServices { get; set; }

    [JsonPropertyName("podcasts")]
    public Podcasts Podcasts { get; set; }

    [JsonPropertyName("articles")]
    public Articles Articles { get; set; }

    [JsonPropertyName("narratives")]
    public Narratives Narratives { get; set; }

    [JsonPropertyName("clips")]
    public Clips Clips { get; set; }

    [JsonPropertyName("clips_followers")]
    public ClipsFollowers ClipsFollowers { get; set; }
}