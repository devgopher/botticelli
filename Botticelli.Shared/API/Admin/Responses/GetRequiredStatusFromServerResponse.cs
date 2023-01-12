namespace Botticelli.Shared.API.Admin.Responses;

public class GetRequiredStatusFromServerResponse
{
    public string? BotId { get; set; }
    public BotStatus? Status { get; set; }
}