using Botticelli.Framework.Vk;
using Botticelli.Interfaces;
using Microsoft.Extensions.Options;
using VkMessagingSample.Settings;

namespace VkMessagingSample;

/// <summary>
///     This hosted service intended for sending messages according to a schedule
/// </summary>
public class TestBotHostedService : IHostedService
{
    private readonly IOptionsMonitor<SampleSettings> _settings;
    private readonly IBot<VkBot> _vkBot;

    public TestBotHostedService(IBot<VkBot> vkBot, IOptionsMonitor<SampleSettings> settings)
    {
        _vkBot = vkBot;
        _settings = settings;
    }

    public Task StartAsync(CancellationToken token)
    {
        Console.WriteLine("Start sending messages...");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stop sending messages...");

        return Task.CompletedTask;
    }
}