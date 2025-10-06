using Botticelli.Server.Back.Services;
using Botticelli.Server.Back.Services.Broadcasting;
using Botticelli.Server.Data.Entities.Bot.Broadcasting;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Utils;
using Botticelli.Shared.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediaType = Botticelli.Shared.Constants.MediaType;

namespace Botticelli.Server.Back.Controllers;

/// <summary>
///     Bot status/data controller
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("/v1/bot")]
public class BotController(
        IBotManagementService botManagementService,
        IBotStatusDataService botStatusDataService,
        IBroadcastService broadcastService,
        ILogger<BotController> logger)
{
    private const int LongPollTimeoutSeconds = 30;
    private const int DefaultPollIntervalMilliseconds = 300;
    
    #region Client pane

    /// <summary>
    ///     Gets a required bot status (active/non-active)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("client/[action]")]
    public async Task<GetRequiredStatusFromServerResponse> GetRequiredBotStatus([FromBody] GetRequiredStatusFromServerRequest request)
    {
        request.NotNull();
        request.BotId?.NotNullOrEmpty();

        var botInfo = await botStatusDataService.GetBotInfo(request.BotId!);
        botInfo.NotNull();
        botInfo?.BotKey?.NotNullOrEmpty();

        var context = new BotContext
        {
            BotId = botInfo!.BotId,
            BotKey = botInfo.BotKey!,
            Items = botInfo.AdditionalInfo?.ToDictionary(k => k.ItemName, k => k.ItemValue)!
        };

        return new GetRequiredStatusFromServerResponse
        {
            BotId = request.BotId!,
            IsSuccess = true,
            Status = await botStatusDataService.GetRequiredBotStatus(request.BotId!),
            BotContext = context
        };
    }

    /// <summary>
    ///     Keep alive function
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("client/[action]")]
    public async Task<KeepAliveNotificationResponse> KeepAlive([FromBody] KeepAliveNotificationRequest request)
    {
        try
        {
            logger.LogTrace("{KeepAliveName}({RequestBotId})...", nameof(KeepAlive), request.BotId);
            request.BotId?.NotNullOrEmpty();
            await botManagementService.SetKeepAlive(request.BotId!);

            return new KeepAliveNotificationResponse
            {
                BotId = request.BotId,
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{KeepAliveName}({RequestBotId}) error: {ExMessage}", nameof(KeepAlive), request.BotId, ex.Message);

            return new KeepAliveNotificationResponse
            {
                BotId = request.BotId,
                IsSuccess = false
            };
        }
    }

    /// <summary>
    ///     Gets broadcast messages
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("client/[action]")]
    public async Task<GetBroadCastMessagesResponse> GetBroadcast([FromBody] GetBroadCastMessagesRequest request)
    {
        try
        {
            request.BotId?.NotNullOrEmpty();

            var broadcastMessages = new List<Broadcast>();
            var started = DateTime.UtcNow;
            
            while (!broadcastMessages.Any() && DateTime.UtcNow.Subtract(started).TotalSeconds < LongPollTimeoutSeconds)
            {
                broadcastMessages = (await broadcastService.GetMessages(request.BotId!)).ToList();
                
                await Task.Delay(DefaultPollIntervalMilliseconds);
            } 
            
            return new GetBroadCastMessagesResponse
            {
                BotId = request.BotId!,
                IsSuccess = true,
                Messages = broadcastMessages.Select(bm => new Message
                                            {
                                                Uid = bm.Id,
                                                Type = Message.MessageType.Messaging,
                                                Subject = string.Empty,
                                                Body = bm.Body,
                                                Attachments = bm.Attachments?.Select(BaseAttachment (att) => new BinaryBaseAttachment(att.Id.ToString(),
                                                                                                                                     att.Filename,
                                                                                                                                     att.MediaType,
                                                                                                                                     string.Empty,
                                                                                                                                     att.Content))
                                                                .ToList() ??
                                                              []
                                            })
                                            .ToArray()
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{GetBroadcastName}({RequestBotId}) error: {ExMessage}", nameof(GetBroadcast), request.BotId, ex.Message);

            return new GetBroadCastMessagesResponse
            {
                BotId = request.BotId!,
                IsSuccess = false,
                Messages = []
            };
        }
    }

    /// <summary>
    ///     Gets broadcast messages received notifications
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("client/[action]")]
    public async Task<BroadCastMessagesReceivedResponse> BroadcastReceived([FromBody] BroadCastMessagesReceivedRequest request)
    {
        try
        {
            logger.LogTrace("{GetBroadcastName}({RequestBotId})...", nameof(GetBroadcast), request.BotId);
            request.BotId?.NotNullOrEmpty();

            foreach (var messageId in request.MessageIds) await broadcastService.MarkReceived(request.BotId!, messageId);

            return new BroadCastMessagesReceivedResponse
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{BroadcastReceivedName}({RequestBotId}) error: {ExMessage}", nameof(BroadcastReceived), request.BotId, ex.Message);

            return new BroadCastMessagesReceivedResponse
            {
                IsSuccess = false
            };
        }
    }

    #endregion
}