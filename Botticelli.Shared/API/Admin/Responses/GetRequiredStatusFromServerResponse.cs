namespace Botticelli.Shared.API.Admin.Responses;

public class GetRequiredStatusFromServerResponse : ServerBaseResponse
{
    public string? BotId { get; set; }
    public BotStatus? Status { get; set; }
}