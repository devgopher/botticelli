using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.Shared.API.Client.Responses;

public class GetRequiredStatusFromServerResponse : ServerBaseResponse
{
    public string? BotId { get; set; }
    public BotStatus? Status { get; set; }

    [Obsolete($"Use BotContext")]
    public string BotKey { get; set; }

    public BotContext BotContext { get; set; }
}