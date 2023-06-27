using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.Constants;

namespace Botticelli.Server.Services;

public interface IBotManagementService
{
    Task<bool> RegisterBot(string botId, string botKey, string botName, BotType botType);
    Task SetRequiredBotStatus(string botId, BotStatus status);
    Task SetKeepAlive(string botId);
    Task RemoveBot(string botId);
}