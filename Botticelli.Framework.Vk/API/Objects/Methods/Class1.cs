using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Access
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Address
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class AgeLimits
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class Articles
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Audio
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class CanInvite
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class CanRecall
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class Categories
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("items")]
        public Items Items { get; set; }
    }

    public class CityId
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class CityName
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Code
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Contacts
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class Count
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("minimum")]
        public int Minimum { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class CountryId
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class CountryName
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Definitions
    {
        [JsonPropertyName("groups_addAddress_response")]
        public GroupsAddAddressResponse GroupsAddAddressResponse { get; set; }

        [JsonPropertyName("groups_addCallbackServer_response")]
        public GroupsAddCallbackServerResponse GroupsAddCallbackServerResponse { get; set; }

        [JsonPropertyName("groups_addLink_response")]
        public GroupsAddLinkResponse GroupsAddLinkResponse { get; set; }

        [JsonPropertyName("groups_create_response")]
        public GroupsCreateResponse GroupsCreateResponse { get; set; }

        [JsonPropertyName("groups_editAddress_response")]
        public GroupsEditAddressResponse GroupsEditAddressResponse { get; set; }

        [JsonPropertyName("groups_getAddresses_response")]
        public GroupsGetAddressesResponse GroupsGetAddressesResponse { get; set; }

        [JsonPropertyName("groups_getBanned_response")]
        public GroupsGetBannedResponse GroupsGetBannedResponse { get; set; }

        [JsonPropertyName("groups_getById_object_legacy_response")]
        public GroupsGetByIdObjectLegacyResponse GroupsGetByIdObjectLegacyResponse { get; set; }

        [JsonPropertyName("groups_getCallbackConfirmationCode_response")]
        public GroupsGetCallbackConfirmationCodeResponse GroupsGetCallbackConfirmationCodeResponse { get; set; }

        [JsonPropertyName("groups_getCallbackServers_response")]
        public GroupsGetCallbackServersResponse GroupsGetCallbackServersResponse { get; set; }

        [JsonPropertyName("groups_getCallbackSettings_response")]
        public GroupsGetCallbackSettingsResponse GroupsGetCallbackSettingsResponse { get; set; }

        [JsonPropertyName("groups_getCatalogInfo_extended_response")]
        public GroupsGetCatalogInfoExtendedResponse GroupsGetCatalogInfoExtendedResponse { get; set; }

        [JsonPropertyName("groups_getCatalogInfo_response")]
        public GroupsGetCatalogInfoResponse GroupsGetCatalogInfoResponse { get; set; }

        [JsonPropertyName("groups_getCatalog_response")]
        public GroupsGetCatalogResponse GroupsGetCatalogResponse { get; set; }

        [JsonPropertyName("groups_getInvitedUsers_response")]
        public GroupsGetInvitedUsersResponse GroupsGetInvitedUsersResponse { get; set; }

        [JsonPropertyName("groups_getInvites_extended_response")]
        public GroupsGetInvitesExtendedResponse GroupsGetInvitesExtendedResponse { get; set; }

        [JsonPropertyName("groups_getInvites_response")]
        public GroupsGetInvitesResponse GroupsGetInvitesResponse { get; set; }

        [JsonPropertyName("groups_getLongPollServer_response")]
        public GroupsGetLongPollServerResponse GroupsGetLongPollServerResponse { get; set; }

        [JsonPropertyName("groups_getLongPollSettings_response")]
        public GroupsGetLongPollSettingsResponse GroupsGetLongPollSettingsResponse { get; set; }

        [JsonPropertyName("groups_getMembers_fields_response")]
        public GroupsGetMembersFieldsResponse GroupsGetMembersFieldsResponse { get; set; }

        [JsonPropertyName("groups_getMembers_filter_response")]
        public GroupsGetMembersFilterResponse GroupsGetMembersFilterResponse { get; set; }

        [JsonPropertyName("groups_getMembers_response")]
        public GroupsGetMembersResponse GroupsGetMembersResponse { get; set; }

        [JsonPropertyName("groups_getRequests_fields_response")]
        public GroupsGetRequestsFieldsResponse GroupsGetRequestsFieldsResponse { get; set; }

        [JsonPropertyName("groups_getRequests_response")]
        public GroupsGetRequestsResponse GroupsGetRequestsResponse { get; set; }

        [JsonPropertyName("groups_getSettings_response")]
        public GroupsGetSettingsResponse GroupsGetSettingsResponse { get; set; }

        [JsonPropertyName("groups_getTagList_response")]
        public GroupsGetTagListResponse GroupsGetTagListResponse { get; set; }

        [JsonPropertyName("groups_getTokenPermissions_response")]
        public GroupsGetTokenPermissionsResponse GroupsGetTokenPermissionsResponse { get; set; }

        [JsonPropertyName("groups_get_object_extended_response")]
        public GroupsGetObjectExtendedResponse GroupsGetObjectExtendedResponse { get; set; }

        [JsonPropertyName("groups_get_response")]
        public GroupsGetResponse GroupsGetResponse { get; set; }

        [JsonPropertyName("groups_isMember_extended_response")]
        public GroupsIsMemberExtendedResponse GroupsIsMemberExtendedResponse { get; set; }

        [JsonPropertyName("groups_isMember_response")]
        public GroupsIsMemberResponse GroupsIsMemberResponse { get; set; }

        [JsonPropertyName("groups_isMember_user_ids_extended_response")]
        public GroupsIsMemberUserIdsExtendedResponse GroupsIsMemberUserIdsExtendedResponse { get; set; }

        [JsonPropertyName("groups_isMember_user_ids_response")]
        public GroupsIsMemberUserIdsResponse GroupsIsMemberUserIdsResponse { get; set; }

        [JsonPropertyName("groups_search_response")]
        public GroupsSearchResponse GroupsSearchResponse { get; set; }
    }

    public class Description
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Docs
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Email
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Enabled
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class EventGroupId
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Events
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class FinishDate
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Groups
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("items")]
        public Items Items { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class GroupsAddAddressResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsAddCallbackServerResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsAddLinkResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsCreateResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsEditAddressResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetAddressesResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetBannedResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetByIdObjectLegacyResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetCallbackConfirmationCodeResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetCallbackServersResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetCallbackSettingsResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetCatalogInfoExtendedResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetCatalogInfoResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetCatalogResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetInvitedUsersResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetInvitesExtendedResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetInvitesResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetLongPollServerResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetLongPollSettingsResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetMembersFieldsResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetMembersFilterResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetMembersResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetObjectExtendedResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetRequestsFieldsResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetRequestsResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetSettingsResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetTagListResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsGetTokenPermissionsResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsIsMemberExtendedResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsIsMemberResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsIsMemberUserIdsExtendedResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsIsMemberUserIdsResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class GroupsSearchResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }

    public class Invitation
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class Items
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("items")]
        public Items Items { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }

        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("entity")]
        public string Entity { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("minimum")]
        public int Minimum { get; set; }
    }

    public class Links
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class MainSection
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class Mask
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("minimum")]
        public int Minimum { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Member
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class ObsceneFilter
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class ObsceneStopwords
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class ObsceneWords
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("items")]
        public Items Items { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Permissions
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("items")]
        public Items Items { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Phone
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Photos
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Profiles
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("items")]
        public Items Items { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

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

    public class PublicCategory
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class PublicCategoryList
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("items")]
        public Items Items { get; set; }
    }

    public class PublicDate
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class PublicDateLabel
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class PublicSubcategory
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class RecognizePhoto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Request
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }

        [JsonPropertyName("items")]
        public Items Items { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("$schema")]
        public string Schema { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("definitions")]
        public Definitions Definitions { get; set; }
    }

    public class Rss
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class SecondarySection
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class SectionsList
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("items")]
        public Items Items { get; set; }
    }

    public class ServerId
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("minimum")]
        public int Minimum { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class StartDate
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("minimum")]
        public int Minimum { get; set; }
    }

    public class Subject
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class SubjectList
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("items")]
        public Items Items { get; set; }
    }

    public class SuggestedPrivacy
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class Title
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Topics
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Twitter
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public class Video
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Wall
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }

    public class Website
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Wiki
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }


}
