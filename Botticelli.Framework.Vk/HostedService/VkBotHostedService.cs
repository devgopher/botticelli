using Botticelli.Interfaces;
using Botticelli.Shared.API.Admin.Requests;
using Microsoft.Extensions.Hosting;

namespace Botticelli.Framework.Vk.Messages.HostedService;

public class VkBotHostedService : IHostedService
{
    private readonly IBot<VkBot> _bot;

    public VkBotHostedService(IBot<VkBot> bot)
    {
        _bot = bot;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
        => await _bot.StartBotAsync(StartBotRequest.GetInstance(), CancellationToken.None);

    public async Task StopAsync(CancellationToken cancellationToken)
        => await _bot.StopBotAsync(StopBotRequest.GetInstance(), CancellationToken.None);
}