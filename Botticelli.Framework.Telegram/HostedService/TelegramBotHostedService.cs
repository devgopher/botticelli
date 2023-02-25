using Botticelli.Interfaces;
using Botticelli.Shared.API.Admin.Requests;
using Microsoft.Extensions.Hosting;

namespace Botticelli.Framework.Telegram.HostedService;

public class TelegramBotHostedService : IHostedService
{
    private readonly IBot<TelegramBot> _bot;

    public TelegramBotHostedService(IBot<TelegramBot> bot) => _bot = bot;

    public async Task StartAsync(CancellationToken cancellationToken)
        => await _bot.StartBotAsync(StartBotRequest.GetInstance(), CancellationToken.None);

    public async Task StopAsync(CancellationToken cancellationToken)
        => await _bot.StopBotAsync(StopBotRequest.GetInstance(), CancellationToken.None);
}