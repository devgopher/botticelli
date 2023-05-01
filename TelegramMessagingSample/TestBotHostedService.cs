using Botticelli.Framework.Telegram;
using Botticelli.Interfaces;
using Botticelli.Scheduler;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Options;
using TelegramMessagingSample.Settings;

namespace TelegramBotSample;

/// <summary>
/// This hosted service intended for sending messages according to a schedule
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

    public async Task StartAsync(CancellationToken token) =>
            JobManager.AddJob(_telegramBot,
                              new Reliability
                              {
                                  IsEnabled = false,
                                  Delay = TimeSpan.FromSeconds(3),
                                  IsExponential = true,
                                  MaxTries = 5
                              },
                              new Message
                              {
                                  Body = "Now you see me!",
                                  Subject = "",
                                  ChatId = _settings.CurrentValue.ChatId,
                                  Attachments = new List<IAttachment>
                                  {
                                      new BinaryAttachment(Guid.NewGuid().ToString(),
                                                           "testpic.png",
                                                           MediaType.Image,
                                                           string.Empty,
                                                           await File.ReadAllBytesAsync("Media/testpic.png", token)),
                                      new BinaryAttachment(Guid.NewGuid().ToString(),
                                                           "voice.mp3",
                                                           MediaType.Voice,
                                                           string.Empty,
                                                           await File.ReadAllBytesAsync("Media/voice.mp3", token)),
                                      new BinaryAttachment(Guid.NewGuid().ToString(),
                                                           "video.mp4",
                                                           MediaType.Video,
                                                           string.Empty,
                                                           await File.ReadAllBytesAsync("Media/video.mp4", token))
                                  }
                              },
                              new Schedule
                              {
                                  Cron = "* * * * *"
                              });

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stop sending messages...");

        return Task.CompletedTask;
    }
}