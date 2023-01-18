using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Shared.API.Client.Responses;

/// <summary>
/// Regisrter bot response from an admin server to a bot
/// </summary>
public class RegisterBotResponse : ServerBaseResponse
{
    public string? BotId { get; set; }

}