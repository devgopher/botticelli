using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Responses;

public class Properties
{
    [JsonPropertyName("response")]
    public Response Response { get; set; }

    [JsonPropertyName("count")]
    public Count Count { get; set; }

    [JsonPropertyName("items")]
    public Items Items { get; set; }

    [JsonPropertyName("code")]
    public Code Code { get; set; }

    [JsonPropertyName("enabled")]
    public Enabled Enabled { get; set; }

    [JsonPropertyName("categories")]
    public Categories Categories { get; set; }

    [JsonPropertyName("profiles")]
    public Profiles Profiles { get; set; }

    [JsonPropertyName("groups")]
    public Groups Groups { get; set; }

    [JsonPropertyName("server_id")]
    public ServerId ServerId { get; set; }

    [JsonPropertyName("access")]
    public Access Access { get; set; }

    [JsonPropertyName("address")]
    public Address Address { get; set; }

    [JsonPropertyName("audio")]
    public Audio Audio { get; set; }

    [JsonPropertyName("articles")]
    public Articles Articles { get; set; }

    [JsonPropertyName("recognize_photo")]
    public RecognizePhoto RecognizePhoto { get; set; }

    [JsonPropertyName("city_id")]
    public CityId CityId { get; set; }

    [JsonPropertyName("city_name")]
    public CityName CityName { get; set; }

    [JsonPropertyName("contacts")]
    public Contacts Contacts { get; set; }

    [JsonPropertyName("links")]
    public Links Links { get; set; }

    [JsonPropertyName("sections_list")]
    public SectionsList SectionsList { get; set; }

    [JsonPropertyName("main_section")]
    public MainSection MainSection { get; set; }

    [JsonPropertyName("secondary_section")]
    public SecondarySection SecondarySection { get; set; }

    [JsonPropertyName("age_limits")]
    public AgeLimits AgeLimits { get; set; }

    [JsonPropertyName("country_id")]
    public CountryId CountryId { get; set; }

    [JsonPropertyName("country_name")]
    public CountryName CountryName { get; set; }

    [JsonPropertyName("description")]
    public Description Description { get; set; }

    [JsonPropertyName("docs")]
    public Docs Docs { get; set; }

    [JsonPropertyName("events")]
    public Events Events { get; set; }

    [JsonPropertyName("obscene_filter")]
    public ObsceneFilter ObsceneFilter { get; set; }

    [JsonPropertyName("obscene_stopwords")]
    public ObsceneStopwords ObsceneStopwords { get; set; }

    [JsonPropertyName("obscene_words")]
    public ObsceneWords ObsceneWords { get; set; }

    [JsonPropertyName("event_group_id")]
    public EventGroupId EventGroupId { get; set; }

    [JsonPropertyName("photos")]
    public Photos Photos { get; set; }

    [JsonPropertyName("public_category")]
    public PublicCategory PublicCategory { get; set; }

    [JsonPropertyName("public_category_list")]
    public PublicCategoryList PublicCategoryList { get; set; }

    [JsonPropertyName("public_date")]
    public PublicDate PublicDate { get; set; }

    [JsonPropertyName("public_date_label")]
    public PublicDateLabel PublicDateLabel { get; set; }

    [JsonPropertyName("public_subcategory")]
    public PublicSubcategory PublicSubcategory { get; set; }

    [JsonPropertyName("rss")]
    public Rss Rss { get; set; }

    [JsonPropertyName("start_date")]
    public StartDate StartDate { get; set; }

    [JsonPropertyName("finish_date")]
    public FinishDate FinishDate { get; set; }

    [JsonPropertyName("subject")]
    public Subject Subject { get; set; }

    [JsonPropertyName("subject_list")]
    public SubjectList SubjectList { get; set; }

    [JsonPropertyName("suggested_privacy")]
    public SuggestedPrivacy SuggestedPrivacy { get; set; }

    [JsonPropertyName("title")]
    public Title Title { get; set; }

    [JsonPropertyName("topics")]
    public Topics Topics { get; set; }

    [JsonPropertyName("twitter")]
    public Twitter Twitter { get; set; }

    [JsonPropertyName("video")]
    public Video Video { get; set; }

    [JsonPropertyName("wall")]
    public Wall Wall { get; set; }

    [JsonPropertyName("website")]
    public Website Website { get; set; }

    [JsonPropertyName("phone")]
    public Phone Phone { get; set; }

    [JsonPropertyName("email")]
    public Email Email { get; set; }

    [JsonPropertyName("wiki")]
    public Wiki Wiki { get; set; }

    [JsonPropertyName("mask")]
    public Mask Mask { get; set; }

    [JsonPropertyName("permissions")]
    public Permissions Permissions { get; set; }

    [JsonPropertyName("member")]
    public Member Member { get; set; }

    [JsonPropertyName("invitation")]
    public Invitation Invitation { get; set; }

    [JsonPropertyName("can_invite")]
    public CanInvite CanInvite { get; set; }

    [JsonPropertyName("can_recall")]
    public CanRecall CanRecall { get; set; }

    [JsonPropertyName("request")]
    public Request Request { get; set; }
}