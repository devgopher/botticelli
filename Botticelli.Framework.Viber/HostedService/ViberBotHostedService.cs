using Botticelli.Interfaces;
using Botticelli.Shared.API.Admin.Requests;
using Microsoft.Extensions.Hosting;

namespace Botticelli.Framework.Viber.HostedService;

public class ViberBotHostedService : IHostedService
{
    private readonly IBot<ViberBot> _bot;

    public ViberBotHostedService(IBot<ViberBot> bot)
    {
        _bot = bot;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _bot.StartBotAsync(StartBotRequest.GetInstance(), CancellationToken.None);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _bot.StopBotAsync(StopBotRequest.GetInstance(), CancellationToken.None);
    }
}