using Botticelli.Server.Data;
using Botticelli.Server.Data.Entities.Bot;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.Constants;

namespace Botticelli.Server.Back.Services;

/// <summary>
///     This class is intended for managing bots state (start/ stop/ block/ remove)
/// </summary>
public class BotManagementService(
        ServerDataContext context,
        ILogger<BotManagementService> logger) : IBotManagementService
{
    /// <summary>
    ///     Registers a bot if it's not registered
    /// </summary>
    /// <param name="botId"></param>
    /// <param name="botKey"></param>
    /// <param name="botName"></param>
    /// <param name="botType"></param>
    /// <param name="additionalParams"></param>
    /// <returns></returns>
    public Task<bool> RegisterBot(string botId,
                                  string? botKey,
                                  string botName,
                                  BotType botType,
                                  Dictionary<string, string>? additionalParams = null)
    {
        try
        {
            logger.LogInformation("{RegisterBotName}({BotId}, {BotKey}, {BotName}, {BotType}) started...", nameof(RegisterBot), botId, botKey, botName, botType);

            if (GetBotInfo(botId) == null)
                AddNewBotInfo(botId,
                              BotStatus.Unknown,
                              botType,
                              botName);

            logger.LogInformation($"{nameof(RegisterBot)} successful");

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }

        logger.LogInformation($"{nameof(RegisterBot)} failed");

        return Task.FromResult(false);
    }

    /// <summary>
    ///     Sets a needed bot status in a database
    /// </summary>
    /// <param name="botId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public async Task SetRequiredBotStatus(string botId, BotStatus status)
    {
        logger.LogInformation($"{nameof(SetRequiredBotStatus)} started");

        var botInfo = GetBotInfo(botId);

        if (botInfo != null)
        {
            botInfo.Status = status;
            context.BotInfos.Update(botInfo);
        }

        await context.SaveChangesAsync();
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
            logger.LogInformation($"{nameof(SetKeepAlive)} started");

            var botInfo = GetBotInfo(botId);

            var keepAlive = DateTime.UtcNow;

            if (botInfo != null)
            {
                botInfo.LastKeepAlive = keepAlive;
                context.BotInfos.Update(botInfo);
            }

            await context.SaveChangesAsync();

            logger.LogInformation($"{nameof(SetKeepAlive)} finished");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
    }

    /// <summary>
    ///     Removes a bot
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    public async Task RemoveBot(string botId)
    {
        await SetRequiredBotStatus(botId, BotStatus.Unlocked);

        var bot = context.BotInfos.FirstOrDefault(b => b.BotId == botId);

        if (bot != null)
        {
            context.BotInfos.Remove(bot);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    ///     Updates a botInfo
    /// </summary>
    /// <param name="botId"></param>
    /// <param name="botKey"></param>
    /// <param name="botName"></param>
    /// <param name="additionalParams"></param>
    /// <returns></returns>
    public async Task<bool> UpdateBot(string botId,
                                      string botKey,
                                      string botName,
                                      Dictionary<string, string>? additionalParams = null)
    {
        try
        {
            logger.LogInformation("{UpdateBotName}({BotId}, {BotKey}, {BotName}) started...", nameof(UpdateBot), botId, botKey, botName);

            var prevStatus = await GetRequiredBotStatus(botId);
            if (prevStatus is not BotStatus.Unlocked) await SetRequiredBotStatus(botId, BotStatus.Unlocked);

            var botInfo = GetBotInfo(botId);

            if (botInfo == null)
            {
                logger.LogInformation("{UpdateBotName}() : bot with id '{BotId}' wasn't found!", nameof(UpdateBot), botId);

                return false;
            }

            botInfo.BotKey = botKey;
            botInfo.BotName = botName;
            // botInfo.Items = additionalParams;

            context.BotInfos.Update(botInfo);
            await context.SaveChangesAsync();

            await SetRequiredBotStatus(botId, prevStatus.Value);

            logger.LogInformation($"{nameof(UpdateBot)} successful");

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }

        logger.LogInformation($"{nameof(UpdateBot)} failed");

        return false;
    }

    /// <summary>
    ///     Gets a bot required status for answering on a poll request from a bot
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    private Task<BotStatus?> GetRequiredBotStatus(string botId)
    {
        return Task.FromResult<BotStatus?>(context.BotInfos.FirstOrDefault(b => b.BotId == botId)?.Status ?? BotStatus.Unknown);
    }

    /// <summary>
    ///     Add a new bot info to a DB
    /// </summary>
    /// <param name="botId"></param>
    /// <param name="status"></param>
    /// <param name="botType"></param>
    /// <param name="botName"></param>
    /// <param name="lastKeepAliveUtc"></param>
    private void AddNewBotInfo(string botId,
                               BotStatus status,
                               BotType botType,
                               string botName,
                               DateTime? lastKeepAliveUtc = null)
    {
        try
        {
            logger.LogInformation($"{nameof(AddNewBotInfo)} started");

            var botInfo = new BotInfo
            {
                BotId = botId,
                BotName = botName,
                LastKeepAlive = lastKeepAliveUtc,
                Status = status,
                Type = botType
            };

            context.BotInfos.Add(botInfo);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }

        logger.LogInformation($"{nameof(AddNewBotInfo)} finished");
    }

    /// <summary>
    ///     Gets info about a bot
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    private BotInfo? GetBotInfo(string botId)
    {
        return context.BotInfos.FirstOrDefault(b => b.BotId == botId);
    }
}