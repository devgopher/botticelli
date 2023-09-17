using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Shared.API.Client.Responses;

/// <summary>
///    Update bot response
/// </summary>
public class UpdateBotResponse : ServerBaseResponse
{
    public string? BotId { get; set; }
}