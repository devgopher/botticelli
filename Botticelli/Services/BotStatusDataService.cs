using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities;
using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Server.Services;

/// <summary>
///     This class is intended for bot management purposes (start/stop/block/add/remove)
/// </summary>
public class BotStatusDataService : IBotStatusDataService
{
    private readonly BotInfoContext _context;

    public BotStatusDataService(BotInfoContext context) => _context = context;

    public ICollection<BotInfo> GetBots() => _context.BotInfos.ToArray();

    /// <summary>
    ///     Gets a bot required status for answering on a poll request from a bot
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    public async Task<BotStatus?> GetRequiredBotStatus(string botId)
        => _context.BotInfos.FirstOrDefault(b => b.BotId == botId)?.Status ?? BotStatus.Unknown;
}