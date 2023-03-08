using Botticelli.Server.Data.Entities;
using Botticelli.Server.Services;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Controllers;

[ApiController]
public class BotController
{
    private readonly IBotManagementService _botManagementService;
    private readonly IBotStatusDataService _botStatusDataService;

    public BotController(IBotManagementService botManagementService, IBotStatusDataService botStatusDataService)
    {
        _botManagementService = botManagementService;
        _botStatusDataService = botStatusDataService;
    }

    #region Admin pane

    [HttpPost("client/[action]")]
    public async Task<RegisterBotResponse> AddNewBot([FromBody] RegisterBotRequest request)
    {
        var success = await _botManagementService.RegisterBot(request.BotId, request.BotKey, request.Type);

        return new RegisterBotResponse
        {
            BotId = request.BotId,
            IsSuccess = success
        };
    }

    [HttpGet("admin/[action]")]
    public async Task<ICollection<BotInfo>> GetBots() => _botStatusDataService.GetBots();

    [HttpGet("admin/[action]")]
    public async Task ActivateBot([FromQuery] string botId) => await _botManagementService.SetRequiredBotStatus(botId, BotStatus.Active);

    [HttpGet("admin/[action]")]
    public async Task DeactivateBot([FromQuery] string botId) => await _botManagementService.SetRequiredBotStatus(botId, BotStatus.NonActive);

    #endregion

    #region Client pane

    [HttpPost("client/[action]")]
    public async Task<GetRequiredStatusFromServerResponse> GetRequiredBotStatus([FromBody] GetRequiredStatusFromServerResponse request) =>
            new()
            {
                BotId = request.BotId,
                IsSuccess = true,
                Status = await _botStatusDataService.GetRequiredBotStatus(request.BotId)
            };

    [HttpPost("client/[action]")]
    public async Task<KeepAliveNotificationResponse> KeepAlive([FromBody] KeepAliveNotificationRequest request)
    {
        try
        {
            await _botManagementService.SetKeepAlive(request.BotId);

            return new KeepAliveNotificationResponse
            {
                BotId = request.BotId,
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            //log

            return new KeepAliveNotificationResponse
            {
                BotId = request.BotId,
                IsSuccess = false
            };
        }
    }

    #endregion
}