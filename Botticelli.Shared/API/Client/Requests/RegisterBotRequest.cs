using Botticelli.Shared.Constants;

namespace Botticelli.Shared.API.Client.Requests;

public class RegisterBotRequest
{
    public string? BotId { get; set; }
    public string? BotKey { get; set; }
    public BotType Type { get; set; }
}