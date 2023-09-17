using Botticelli.Shared.Constants;

namespace Botticelli.Shared.API.Client.Requests;

public class UpdateBotRequest
{
    public string BotId { get; set; }
    public string BotKey { get; set; }
    public string BotName { get; set; }
}