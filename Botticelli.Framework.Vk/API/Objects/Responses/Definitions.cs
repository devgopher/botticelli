using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Responses;

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