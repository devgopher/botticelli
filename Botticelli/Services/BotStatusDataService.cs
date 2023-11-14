using BotDataSecureStorage;
using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.Server.Services;

/// <summary>
///     This class is intended for bot management purposes (start/stop/block/add/remove)
/// </summary>
public class BotStatusDataService : IBotStatusDataService
{
    private readonly BotInfoContext _context;
    private readonly SecureStorage _secureStorage;

    public BotStatusDataService(BotInfoContext context, SecureStorage secureStorage)
    {
        _context = context;
        _secureStorage = secureStorage;
    }

    public ICollection<BotInfo> GetBots() => _context.BotInfos.ToArray();

    /// <summary>
    ///     Gets a bot required status for answering on a poll request from a bot
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    public async Task<BotStatus?> GetRequiredBotStatus(string botId)
        => _context.BotInfos.FirstOrDefault(b => b.BotId == botId)?.Status ?? BotStatus.Unknown;

    [Obsolete("Use GetRequiredBotContext")]
    public async Task<string> GetRequiredBotKey(string botId) => _secureStorage.GetBotContext(botId)?.BotKey;

    public async Task<BotContext> GetRequiredBotContext(string botId)
    {
        _secureStorage.MigrateToBotContext(botId);

        return _secureStorage.GetBotContext(botId);
    }
}