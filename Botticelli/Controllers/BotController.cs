using Botticelli.Server.Services;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
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

    [HttpGet("admin/[action]")]
    public async Task SetBotActiveStatus([FromQuery] string botId)
        => await _botManagementService.SetRequiredBotStatus(botId, BotStatus.Active);


    [HttpGet("admin/[action]")]
    public async Task SetBotNonActiveStatus([FromQuery] string botId)
        => await _botManagementService.SetRequiredBotStatus(botId, BotStatus.NonActive);
    #endregion

    #region Client pane

    [HttpPost("client/[action]")]
    public async Task<GetRequiredStatusFromServerResponse> GetRequiredBotStatus(
        [FromBody] GetRequiredStatusFromServerResponse request)
        => new()
        {
            BotId = request.BotId,
            IsSuccess = true,
            Status = await _botStatusDataService.GetRequiredBotStatus(request.BotId)
        };


    [HttpPost("client/[action]")]
    public async Task<KeepAliveNotificationResponse> KeepAlive(
        [FromBody] KeepAliveNotificationRequest request)
    {
        try
        {
            await _botManagementService.SetKeepAlive(request.BotId);

            return new()
            {
                BotId = request.BotId,
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            //log

            return new()
            {
                BotId = request.BotId,
                IsSuccess = false
            };
        }
    }

    #endregion
}