using System.Text;
using System.Text.Json;
using Botticelli.Server.Back.Services;
using Botticelli.Server.Back.Services.Broadcasting;
using Botticelli.Server.Data.Entities.Bot;
using Botticelli.Server.Data.Entities.Bot.Broadcasting;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Botticelli.Server.Back.Controllers;

/// <summary>
///     Admin controller getting/adding/removing bots
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
[Route("/v1/admin")]
public class AdminController(
        IBotManagementService botManagementService,
        IBotStatusDataService botStatusDataService,
        ILogger<AdminController> logger,
        IBroadcastService broadcastService) : ControllerBase
{
    /// <summary>
    ///     Adds a new bot
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    public async Task<RegisterBotResponse> AddNewBot([FromBody] RegisterBotRequest request)
    {
        logger.LogInformation("{AddNewBotName}({RequestBotId}) started...", nameof(AddNewBot), request.BotId);
        var success = await botManagementService.RegisterBot(request.BotId,
                                                             request.BotKey,
                                                             request.BotName,
                                                             request.Type);

        logger.LogInformation("{AddNewBotName}({RequestBotId}) success: {Success}...",
                              nameof(AddNewBot),
                              request.BotId,
                              success);

        return new RegisterBotResponse
        {
            BotId = request.BotId,
            IsSuccess = success
        };
    }

    /// <summary>
    /// Updates a bot
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("[action]")]
    public async Task<UpdateBotResponse> UpdateBot([FromBody] UpdateBotRequest request)
    {
        logger.LogInformation("{UpdateBotName}({RequestBotId}) started...", nameof(UpdateBot), request.BotId);
        var success = await botManagementService.UpdateBot(request.BotId,
                                                           request.BotKey,
                                                           request.BotName);

        logger.LogInformation("{UpdateBotName}({RequestBotId}) success: {Success}...",
                              nameof(UpdateBot),
                              request.BotId,
                              success);

        return new UpdateBotResponse
        {
            BotId = request.BotId,
            IsSuccess = success
        };
    }

    /// <summary>
    ///     Sends a broadcast message
    /// </summary>
    /// <param name="botId"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    public async Task SendBroadcast([FromQuery] string botId, [FromBody] Message message)
    {
        await DoBroadcast(botId, message);
    }

    /// <summary>
    ///     Sends a broadcast message in binary stream
    /// </summary>
    /// <param name="botId"></param>
    /// <param name="stream"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    public async Task SendBroadcastBinary([FromQuery] string botId)
    {
        using var memoryStream = new MemoryStream();
        await Request.Body.CopyToAsync(memoryStream);

        var message = JsonSerializer.Deserialize<Message>(Encoding.UTF8.GetString(memoryStream.ToArray()));

        await DoBroadcast(botId, message);
    }

    private async Task DoBroadcast(string botId, Message? message)
    {
        await broadcastService.BroadcastMessage(new Broadcast
        {
            Id = message?.Uid ?? throw new NullReferenceException("Id cannot be null!"),
            BotId = botId ?? throw new NullReferenceException("BotId cannot be null!"),
            Body = message.Body ?? throw new NullReferenceException("Body cannot be null!"),
            Attachments = message.Attachments
                                 .Where(a => a is BinaryBaseAttachment)
                                 .Select(a =>
                                 {
                                     if (a is BinaryBaseAttachment baseAttachment)
                                         return new BroadcastAttachment
                                         {
                                             Id = Guid.NewGuid(),
                                             BroadcastId = message.Uid,
                                             MediaType = baseAttachment.MediaType,
                                             Filename = baseAttachment.Name,
                                             Content = baseAttachment.Data
                                         };

                                     return null!;
                                 })
                                 .ToList(),
            Sent = true
        });
    }

    /// <summary>
    ///     Get bots list
    /// </summary>
    /// <returns></returns>
    [HttpGet("[action]")]
    public Task<ICollection<BotInfo>> GetBots()
    {
        return Task.FromResult(botStatusDataService.GetBots());
    }

    /// <summary>
    ///     Activates a bot (BotStatus.Unlocked)
    /// </summary>
    /// <param name="botId"></param>
    [HttpGet("[action]")]
    public async Task ActivateBot([FromQuery] string botId)
    {
        await botManagementService.SetRequiredBotStatus(botId, BotStatus.Unlocked);
    }

    /// <summary>
    ///     Deactivates a bot (BotStatus.Locked)
    /// </summary>
    /// <param name="botId"></param>
    [HttpGet("[action]")]
    public async Task DeactivateBot([FromQuery] string botId)
    {
        await botManagementService.SetRequiredBotStatus(botId, BotStatus.Locked);
    }

    /// <summary>
    ///     Removes a bot
    /// </summary>
    /// <param name="botId"></param>
    [HttpGet("[action]")]
    public async Task RemoveBot([FromQuery] string botId)
    {
        await botManagementService.RemoveBot(botId);
    }
}