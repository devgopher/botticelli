using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.Constants;

namespace Botticelli.Server.Services;

public interface IBotManagementService
{
    Task SetRequiredBotStatus(string botId, BotStatus status, BotType botType); 
    Task SetKeepAlive(string botId, BotType botType);
}