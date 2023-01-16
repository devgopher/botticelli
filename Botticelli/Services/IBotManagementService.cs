using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Server.Services;

public interface IBotManagementService
{

    Task SetRequiredBotStatus(string botId, BotStatus status); 
    Task SetKeepAlive(string botId);
}