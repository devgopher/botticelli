using Botticelli.Server.Data.Entities;
using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Server.Services;

public interface IBotStatusDataService
{
    /// <summary>
    ///     Gets a collection of existing bots
    /// </summary>
    /// <returns></returns>
    Task<ICollection<BotInfo>> GetBots();

    /// <summary>
    ///     Gets a bot required status for answering on a poll request from a bot
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    Task<BotStatus?> GetRequiredBotStatus(string botId);
}