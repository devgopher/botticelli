using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Shared.API.Client.Responses;

public class GetRequiredStatusFromServerResponse : ServerBaseResponse
{
    public string? BotId { get; set; }
    public BotStatus? Status { get; set; }
}