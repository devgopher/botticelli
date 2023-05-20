using Botticelli.Server.Services;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Controllers;

/// <summary>
/// Bot status controller
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("bot")]
public class BotController
{
    private readonly IBotManagementService _botManagementService;
    private readonly IBotStatusDataService _botStatusDataService;

    public BotController(IBotManagementService botManagementService,
                         IBotStatusDataService botStatusDataService)
    {
        _botManagementService = botManagementService;
        _botStatusDataService = botStatusDataService;
    }
    
    #region Client pane

    /// <summary>
    /// Gets a required bot status (active/non-active)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("client/[action]")]
    public async Task<GetRequiredStatusFromServerResponse> GetRequiredBotStatus([FromBody] GetRequiredStatusFromServerRequest request) =>
            new()
            {
                BotId = request.BotId,
                IsSuccess = true,
                Status = await _botStatusDataService.GetRequiredBotStatus(request.BotId)
            };

    /// <summary>
    /// Keep alive function
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
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