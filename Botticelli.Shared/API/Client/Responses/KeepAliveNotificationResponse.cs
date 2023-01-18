using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Shared.API.Client.Responses;

/// <summary>
/// Keep alive response from an admin server to a bot
/// </summary>
public class KeepAliveNotificationResponse : ServerBaseResponse
{
    public string? BotId { get; set; }

}