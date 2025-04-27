using Botticelli.Framework.Options;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Admin.Requests;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Services;

public class BotStandaloneService(
    IHttpClientFactory httpClientFactory,
    ServerSettings serverSettings,
    BotData.Entities.Bot.BotData botData,
    IBot bot,
    ILogger<BotStatusService> logger)
    : BotActualizationService(httpClientFactory,
        serverSettings,
        bot,
        logger)
{
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await Bot.SetBotContext(botData, cancellationToken);
        await Bot.StartBotAsync(StartBotRequest.GetInstance(), cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken) 
        => await Bot.StopBotAsync(StopBotRequest.GetInstance(), cancellationToken);
}