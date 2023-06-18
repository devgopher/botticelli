using Botticelli.Interfaces;
using Botticelli.Shared.API.Admin.Requests;
using Microsoft.Extensions.Hosting;

namespace Botticelli.Framework.WhatsApp.HostedService;

public class WhatsAppBotHostedService : IHostedService
{
    private readonly IBot<WhatsAppBot> _bot;

    public WhatsAppBotHostedService(IBot<WhatsAppBot> bot) => _bot = bot;

    public async Task StartAsync(CancellationToken cancellationToken)
        => await _bot.StartBotAsync(StartBotRequest.GetInstance(), CancellationToken.None);

    public async Task StopAsync(CancellationToken cancellationToken)
        => await _bot.StopBotAsync(StopBotRequest.GetInstance(), CancellationToken.None);
}