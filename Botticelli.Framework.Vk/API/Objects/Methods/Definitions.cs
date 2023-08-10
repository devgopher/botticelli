using System.Text.Json.Serialization;

namespace Botticelli.Framework.Vk.API.Objects.Methods;

public class Definitions
{
    [JsonPropertyName("groups_address")]
    public GroupsAddress GroupsAddress { get; set; }

    [JsonPropertyName("groups_address_timetable")]
    public GroupsAddressTimetable GroupsAddressTimetable { get; set; }

    [JsonPropertyName("groups_address_timetable_day")]
    public GroupsAddressTimetableDay GroupsAddressTimetableDay { get; set; }

    [JsonPropertyName("groups_address_work_info_status")]
    public GroupsAddressWorkInfoStatus GroupsAddressWorkInfoStatus { get; set; }

    [JsonPropertyName("groups_addresses_info")]
    public GroupsAddressesInfo GroupsAddressesInfo { get; set; }

    [JsonPropertyName("groups_ban_info")]
    public GroupsBanInfo GroupsBanInfo { get; set; }

    [JsonPropertyName("groups_ban_info_reason")]
    public GroupsBanInfoReason GroupsBanInfoReason { get; set; }

    [JsonPropertyName("groups_banned_item")]
    public GroupsBannedItem GroupsBannedItem { get; set; }

    [JsonPropertyName("groups_callback_server")]
    public GroupsCallbackServer GroupsCallbackServer { get; set; }

    [JsonPropertyName("groups_callback_settings")]
    public GroupsCallbackSettings GroupsCallbackSettings { get; set; }

    [JsonPropertyName("groups_contacts_item")]
    public GroupsContactsItem GroupsContactsItem { get; set; }

    [JsonPropertyName("groups_counters_group")]
    public GroupsCountersGroup GroupsCountersGroup { get; set; }

    [JsonPropertyName("groups_cover")]
    public GroupsCover GroupsCover { get; set; }

    [JsonPropertyName("groups_fields")]
    public GroupsFields GroupsFields { get; set; }

    [JsonPropertyName("groups_filter")]
    public GroupsFilter GroupsFilter { get; set; }

    [JsonPropertyName("groups_group")]
    public GroupsGroup GroupsGroup { get; set; }

    [JsonPropertyName("groups_group_access")]
    public GroupsGroupAccess GroupsGroupAccess { get; set; }

    [JsonPropertyName("groups_group_admin_level")]
    public GroupsGroupAdminLevel GroupsGroupAdminLevel { get; set; }

    [JsonPropertyName("groups_group_age_limits")]
    public GroupsGroupAgeLimits GroupsGroupAgeLimits { get; set; }

    [JsonPropertyName("groups_group_attach")]
    public GroupsGroupAttach GroupsGroupAttach { get; set; }

    [JsonPropertyName("groups_group_audio")]
    public GroupsGroupAudio GroupsGroupAudio { get; set; }

    [JsonPropertyName("groups_group_ban_info")]
    public GroupsGroupBanInfo GroupsGroupBanInfo { get; set; }

    [JsonPropertyName("groups_group_category")]
    public GroupsGroupCategory GroupsGroupCategory { get; set; }

    [JsonPropertyName("groups_group_category_full")]
    public GroupsGroupCategoryFull GroupsGroupCategoryFull { get; set; }

    [JsonPropertyName("groups_group_category_type")]
    public GroupsGroupCategoryType GroupsGroupCategoryType { get; set; }

    [JsonPropertyName("groups_group_docs")]
    public GroupsGroupDocs GroupsGroupDocs { get; set; }

    [JsonPropertyName("groups_group_full")]
    public GroupsGroupFull GroupsGroupFull { get; set; }

    [JsonPropertyName("groups_group_full_age_limits")]
    public GroupsGroupFullAgeLimits GroupsGroupFullAgeLimits { get; set; }

    [JsonPropertyName("groups_group_full_member_status")]
    public GroupsGroupFullMemberStatus GroupsGroupFullMemberStatus { get; set; }

    [JsonPropertyName("groups_group_full_section")]
    public GroupsGroupFullSection GroupsGroupFullSection { get; set; }

    [JsonPropertyName("groups_group_is_closed")]
    public GroupsGroupIsClosed GroupsGroupIsClosed { get; set; }

    [JsonPropertyName("groups_group_market_currency")]
    public GroupsGroupMarketCurrency GroupsGroupMarketCurrency { get; set; }

    [JsonPropertyName("groups_group_photos")]
    public GroupsGroupPhotos GroupsGroupPhotos { get; set; }

    [JsonPropertyName("groups_group_public_category_list")]
    public GroupsGroupPublicCategoryList GroupsGroupPublicCategoryList { get; set; }

    [JsonPropertyName("groups_group_role")]
    public GroupsGroupRole GroupsGroupRole { get; set; }

    [JsonPropertyName("groups_group_subject")]
    public GroupsGroupSubject GroupsGroupSubject { get; set; }

    [JsonPropertyName("groups_group_suggested_privacy")]
    public GroupsGroupSuggestedPrivacy GroupsGroupSuggestedPrivacy { get; set; }

    [JsonPropertyName("groups_group_tag")]
    public GroupsGroupTag GroupsGroupTag { get; set; }

    [JsonPropertyName("groups_group_topics")]
    public GroupsGroupTopics GroupsGroupTopics { get; set; }

    [JsonPropertyName("groups_group_type")]
    public GroupsGroupType GroupsGroupType { get; set; }

    [JsonPropertyName("groups_group_video")]
    public GroupsGroupVideo GroupsGroupVideo { get; set; }

    [JsonPropertyName("groups_group_wall")]
    public GroupsGroupWall GroupsGroupWall { get; set; }

    [JsonPropertyName("groups_group_wiki")]
    public GroupsGroupWiki GroupsGroupWiki { get; set; }

    [JsonPropertyName("groups_groups_array")]
    public GroupsGroupsArray GroupsGroupsArray { get; set; }

    [JsonPropertyName("groups_links_item")]
    public GroupsLinksItem GroupsLinksItem { get; set; }

    [JsonPropertyName("groups_live_covers")]
    public GroupsLiveCovers GroupsLiveCovers { get; set; }

    [JsonPropertyName("groups_long_poll_events")]
    public GroupsLongPollEvents GroupsLongPollEvents { get; set; }

    [JsonPropertyName("groups_long_poll_server")]
    public GroupsLongPollServer GroupsLongPollServer { get; set; }

    [JsonPropertyName("groups_long_poll_settings")]
    public GroupsLongPollSettings GroupsLongPollSettings { get; set; }

    [JsonPropertyName("groups_market_info")]
    public GroupsMarketInfo GroupsMarketInfo { get; set; }

    [JsonPropertyName("groups_market_state")]
    public GroupsMarketState GroupsMarketState { get; set; }

    [JsonPropertyName("groups_member_role")]
    public GroupsMemberRole GroupsMemberRole { get; set; }

    [JsonPropertyName("groups_member_role_permission")]
    public GroupsMemberRolePermission GroupsMemberRolePermission { get; set; }

    [JsonPropertyName("groups_member_role_status")]
    public GroupsMemberRoleStatus GroupsMemberRoleStatus { get; set; }

    [JsonPropertyName("groups_member_status")]
    public GroupsMemberStatus GroupsMemberStatus { get; set; }

    [JsonPropertyName("groups_member_status_full")]
    public GroupsMemberStatusFull GroupsMemberStatusFull { get; set; }

    [JsonPropertyName("groups_online_status")]
    public GroupsOnlineStatus GroupsOnlineStatus { get; set; }

    [JsonPropertyName("groups_online_status_type")]
    public GroupsOnlineStatusType GroupsOnlineStatusType { get; set; }

    [JsonPropertyName("groups_owner_xtr_ban_info")]
    public GroupsOwnerXtrBanInfo GroupsOwnerXtrBanInfo { get; set; }

    [JsonPropertyName("groups_owner_xtr_ban_info_type")]
    public GroupsOwnerXtrBanInfoType GroupsOwnerXtrBanInfoType { get; set; }

    [JsonPropertyName("groups_photo_size")]
    public GroupsPhotoSize GroupsPhotoSize { get; set; }

    [JsonPropertyName("groups_role_options")]
    public GroupsRoleOptions GroupsRoleOptions { get; set; }

    [JsonPropertyName("groups_sections_list_item")]
    public GroupsSectionsListItem GroupsSectionsListItem { get; set; }

    [JsonPropertyName("groups_settings_twitter")]
    public GroupsSettingsTwitter GroupsSettingsTwitter { get; set; }

    [JsonPropertyName("groups_subject_item")]
    public GroupsSubjectItem GroupsSubjectItem { get; set; }

    [JsonPropertyName("groups_token_permission_setting")]
    public GroupsTokenPermissionSetting GroupsTokenPermissionSetting { get; set; }

    [JsonPropertyName("groups_user_xtr_role")]
    public GroupsUserXtrRole GroupsUserXtrRole { get; set; }
}