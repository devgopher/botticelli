using BotDataSecureStorage;
using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.Constants;

namespace Botticelli.Server.Services;

/// <summary>
///     This class is intended for managing bots state (start/ stop/ block/ remove)
/// </summary>
public class BotManagementService : IBotManagementService
{
    private readonly BotInfoContext _context;
    private readonly ILogger<BotManagementService> _logger;
    private readonly SecureStorage _secureStorage;

    public BotManagementService(BotInfoContext context,
                                SecureStorage secureStorage,
                                ILogger<BotManagementService> logger)
    {
        _context = context;
        _secureStorage = secureStorage;
        _logger = logger;
    }

    /// <summary>
    ///     Registers a bot if it's not registered
    /// </summary>
    /// <param name="botId"></param>
    /// <param name="botKey"></param>
    /// <param name="botName"></param>
    /// <param name="botType"></param>
    /// <returns></returns>
    public async Task<bool> RegisterBot(string botId, string botKey, string botName, BotType botType)
    {
        try
        {
            _logger.LogInformation($"{nameof(RegisterBot)}({botId}, {botKey}, {botName}, {botType}) started...");

            botKey ??= _secureStorage.GetBotKey(botId).Key;

            if (GetBotInfo(botId) == default)
                AddNewBotInfo(botId,
                              BotStatus.Unknown,
                              botType,
                              botName);

            _secureStorage.SetBotKey(botId, botKey);

            _logger.LogInformation($"{nameof(RegisterBot)} successful");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        _logger.LogInformation($"{nameof(RegisterBot)} failed");

        return false;
    }

    /// <summary>
    ///     Sets a needed bot status in a database
    /// </summary>
    /// <param name="botId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public async Task SetRequiredBotStatus(string botId, BotStatus status)
    {
        _logger.LogInformation($"{nameof(SetRequiredBotStatus)} started");

        var botInfo = GetBotInfo(botId);

        if (botInfo != default)
        {
            botInfo.Status = status;
            _context.BotInfos.Update(botInfo);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    ///     Set keep alive mark
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    public async Task SetKeepAlive(string botId)
    {
        try
        {
            _logger.LogInformation($"{nameof(SetKeepAlive)} started");

            var botInfo = GetBotInfo(botId);

            var keepAlive = DateTime.UtcNow;

            if (botInfo != default)
            {
                botInfo.LastKeepAlive = keepAlive;
                _context.BotInfos.Update(botInfo);
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation($"{nameof(SetKeepAlive)} finished");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    /// <summary>
    ///     Gets a bot required status for answering on a poll request from a bot
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    public async Task<BotStatus?> GetRequiredBotStatus(string botId)
        => _context.BotInfos.FirstOrDefault(b => b.BotId == botId)?.Status ?? BotStatus.Unknown;

    public async Task RemoveBot(string botId)
    {
        await SetRequiredBotStatus(botId, BotStatus.NonActive);

        var bot = _context.BotInfos.FirstOrDefault(b => b.BotId == botId);

        if (bot != default)
        {
            _context.BotInfos.Remove(bot);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    ///     Add a new bot info to a DB
    /// </summary>
    /// <param name="botId"></param>
    /// <param name="status"></param>
    /// <param name="botType"></param>
    /// <param name="lastKeepAliveUtc"></param>
    private void AddNewBotInfo(string botId,
                               BotStatus status,
                               BotType botType,
                               string botName,
                               DateTime? lastKeepAliveUtc = null)
    {
        try
        {
            _logger.LogInformation($"{nameof(AddNewBotInfo)} started");

            var botInfo = new BotInfo
            {
                BotId = botId,
                BotName = botName,
                LastKeepAlive = lastKeepAliveUtc,
                Status = status,
                Type = botType
            };

            _context.BotInfos.Add(botInfo);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        _logger.LogInformation($"{nameof(AddNewBotInfo)} finished");
    }

    /// <summary>
    ///     Gets info about a bot
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    private BotInfo? GetBotInfo(string botId) => _context.BotInfos.FirstOrDefault(b => b.BotId == botId);
}