namespace Botticelli.Shared.API.Admin.Responses;

/// <summary>
/// Keep alive response from an admin server to a bot
/// </summary>
public class KeepAliveNotificationResponse
{
    public string? BotId { get; set; }
    public bool IsSuccess { get; set; }
}