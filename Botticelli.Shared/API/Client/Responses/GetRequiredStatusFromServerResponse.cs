using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.Shared.API.Client.Responses;

public class GetRequiredStatusFromServerResponse : ServerBaseResponse
{
    public required string BotId { get; set; }
    public BotStatus? Status { get; set; }

    public required BotContext BotContext { get; set; }
}