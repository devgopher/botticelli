using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities.Bot;
using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Server.Back.Services;

/// <summary>
///     This class is intended for bot management purposes (Getting a bots list/context/status)
/// </summary>
public class BotStatusDataService(ServerDataContext context) : IBotStatusDataService
{
    private readonly ServerDataContext _context = context;

    public ICollection<BotInfo> GetBots()
    {
        return _context.BotInfos.ToArray();
    }

    /// <summary>
    ///     Gets a bot required status for answering on a poll request from a bot
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    public Task<BotStatus?> GetRequiredBotStatus(string botId)
    {
        return Task.FromResult<BotStatus?>(_context.BotInfos.FirstOrDefault(b => b.BotId == botId)?.Status ??
                                           BotStatus.Unknown);
    }

    [Obsolete("Use GetRequiredBotContext")]
    public Task<string> GetRequiredBotKey(string botId)
    {
        return Task.FromResult(_context.BotInfos.FirstOrDefault(bi => bi.BotId == botId)?.BotKey ?? string.Empty);
    }

    public Task<BotInfo?> GetBotInfo(string botId)
    {
        return Task.FromResult(_context.BotInfos.FirstOrDefault(bi => bi.BotId == botId));
    }
}