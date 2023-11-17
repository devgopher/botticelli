using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Shared.API.Client.Responses;

/// <summary>
///     Register bot response
/// </summary>
public class RegisterBotResponse : ServerBaseResponse
{
    public string? BotId { get; set; }
}