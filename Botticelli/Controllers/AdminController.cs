using Botticelli.Server.Data.Entities;
using Botticelli.Server.Services;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class AdminController
{
    private readonly IBotManagementService _botManagementService;
    private readonly IBotStatusDataService _botStatusDataService;

    public AdminController(IBotManagementService botManagementService,
                         IBotStatusDataService botStatusDataService)
    {
        _botManagementService = botManagementService;
        _botStatusDataService = botStatusDataService;
    }

    [HttpPost("[action]")]
    public async Task<RegisterBotResponse> AddNewBot([FromBody] RegisterBotRequest request)
    {
        var success = await _botManagementService.RegisterBot(request.BotId, request.BotKey, request.Type);

        return new RegisterBotResponse
        {
            BotId = request.BotId,
            IsSuccess = success
        };
    }

    [HttpGet("[action]")]
    public async Task<ICollection<BotInfo>> GetBots() => _botStatusDataService.GetBots();

    [HttpGet("[action]")]
    public async Task ActivateBot([FromQuery] string botId) => await _botManagementService.SetRequiredBotStatus(botId, BotStatus.Active);

    [HttpGet("[action]")]
    public async Task DeactivateBot([FromQuery] string botId) => await _botManagementService.SetRequiredBotStatus(botId, BotStatus.NonActive);


}