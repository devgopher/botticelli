using Botticelli.Server.Data.Entities;
using Botticelli.Server.Services;
using Botticelli.Server.Services.Auth;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Controllers;

/// <summary>
/// Admin controller getting/adding/removing bots
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
[Route("admin")]
public class AdminController
{
    private readonly IBotManagementService _botManagementService;
    private readonly IBotStatusDataService _botStatusDataService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IBotManagementService botManagementService,
                         IBotStatusDataService botStatusDataService,
                         ILogger<AdminController> logger)
    {
        _botManagementService = botManagementService;
        _botStatusDataService = botStatusDataService;
        _logger = logger;
    }

    [HttpPost("[action]")]
    public async Task<RegisterBotResponse> AddNewBot([FromBody] RegisterBotRequest request)
    {
        _logger.LogInformation($"{nameof(AddNewBot)}({request.BotId}) started...");
        var success = await _botManagementService.RegisterBot(request.BotId, request.BotKey, request.Type);

        _logger.LogInformation($"{nameof(AddNewBot)}({request.BotId}) success: {success}...");
        return new RegisterBotResponse
        {
            BotId = request.BotId,
            IsSuccess = success
        };
    }

    [HttpGet("[action]")]
    public async Task<ICollection<BotInfo>> GetBots() 
        => _botStatusDataService.GetBots();

    [HttpGet("[action]")]
    public async Task ActivateBot([FromQuery] string botId) 
        => await _botManagementService.SetRequiredBotStatus(botId, BotStatus.Active);

    [HttpGet("[action]")]
    public async Task DeactivateBot([FromQuery] string botId) 
        => await _botManagementService.SetRequiredBotStatus(botId, BotStatus.NonActive);


}