namespace Botticelli.Shared.API.Admin.Requests;

/// <summary>
/// Requests a needed status from a server (started/stopped)
/// </summary>
public class GetRequiredStatusFromServerRequest
{
    public string? BotId { get; set; }
}