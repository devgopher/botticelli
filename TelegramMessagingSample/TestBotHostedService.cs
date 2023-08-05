using Botticelli.Framework.Telegram;
using Botticelli.Interfaces;
using Botticelli.Scheduler;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Options;
using TelegramMessagingSample.Settings;

namespace TelegramMessagingSample;

/// <summary>
///     This hosted service intended for sending messages according to a schedule
/// </summary>
public class TestBotHostedService : IHostedService
{
    private readonly IOptionsMonitor<SampleSettings> _settings;
    private readonly IBot<TelegramBot> _telegramBot;

    public TestBotHostedService(IBot<TelegramBot> telegramBot, IOptionsMonitor<SampleSettings> settings)
    {
        _telegramBot = telegramBot;
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