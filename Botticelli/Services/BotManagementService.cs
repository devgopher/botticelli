﻿using BotDataSecureStorage;
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
    private readonly SecureStorage _secureStorage; 

    public BotManagementService(BotInfoContext context, SecureStorage secureStorage)
    {
        _context = context;
        _secureStorage = secureStorage;
    }

    /// <summary>
    ///     Registers a bot if it's not registered
    /// </summary>
    /// <param name="botId"></param>
    /// <param name="botKey"></param>
    /// <param name="botType"></param>
    /// <returns></returns>
    public async Task<bool> RegisterBot(string botId, string botKey, BotType botType)
    {
        try
        {
            if (GetBotInfo(botId) == default)
                AddNewBotInfo(botId, botKey, BotStatus.Unknown, botType);

            _secureStorage.SetBotKey(botId, botKey);
            //var tst = _secureStorage.GetBotKey(botId);
            return true;
        }
        catch (Exception ex)
        {
            // todo: log
        }

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
        var botInfo = GetBotInfo(botId);

        var keepAlive = DateTime.UtcNow;

        if (botInfo != default)
        {
            botInfo.LastKeepAlive = keepAlive;
            _context.BotInfos.Update(botInfo);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    ///     Gets a bot required status for answering on a poll request from a bot
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    public async Task<BotStatus?> GetRequiredBotStatus(string botId) 
        => _context.BotInfos.FirstOrDefault(b => b.BotId == botId)?.Status ?? BotStatus.Unknown;

    /// <summary>
    ///     Add a new bot info to a DB
    /// </summary>
    /// <param name="botId"></param>
    /// <param name="botKey"></param>
    /// <param name="status"></param>
    /// <param name="botType"></param>
    /// <param name="lastKeepAliveUtc"></param>
    private void AddNewBotInfo(string botId,
                               string botKey,
                               BotStatus status,
                               BotType botType,
                               DateTime? lastKeepAliveUtc = null)
    {
        try
        {
            var botInfo = new BotInfo
            {
                BotId = botId,
                LastKeepAlive = lastKeepAliveUtc,
                Status = status,
                Type = botType
            };

            _context.BotInfos.Add(botInfo);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            // todo:
        }
    }

    /// <summary>
    ///     Gets info about a bot
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    private BotInfo? GetBotInfo(string botId) => _context.BotInfos.FirstOrDefault(b => b.BotId == botId);
}