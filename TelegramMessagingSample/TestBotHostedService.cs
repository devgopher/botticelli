using Botticelli.Framework.Telegram;
using Botticelli.Interfaces;
using Botticelli.Scheduler;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using TelegramMessagingSample.Settings;

namespace TelegramBotSample;

public class TestBotHostedService : IHostedService
{
    private readonly SampleSettings _settings;
    private readonly IBot<TelegramBot> _telegramBot;

    public TestBotHostedService(IBot<TelegramBot> telegramBot, SampleSettings settings)
    {
        _telegramBot = telegramBot;
        _settings = settings;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        JobManager.AddJob(_telegramBot,
                          new Reliability
                          {
                              IsEnabled = false,
                              Delay = TimeSpan.FromSeconds(3),
                              IsExponential = true,
                              MaxTries = 5
                          },
                          new Message("testid")
                          {
                              Body = "testmsg",
                              Subject = "subj",
                              ChatId = _settings.ChatId,
                              Attachments = new List<IAttachment>
                              {
                                  new BinaryAttachment(Guid.NewGuid().ToString(),
                                                       "testpic.png",
                                                       MediaType.Image,
                                                       string.Empty,
                                                       File.ReadAllBytes("Media/testpic.png")),
                                  new BinaryAttachment(Guid.NewGuid().ToString(),
                                                       "voice.mp3",
                                                       MediaType.Voice,
                                                       string.Empty,
                                                       File.ReadAllBytes("Media/voice.mp3")),
                                  new BinaryAttachment(Guid.NewGuid().ToString(),
                                                       "video.mp4",
                                                       MediaType.Video,
                                                       string.Empty,
                                                       File.ReadAllBytes("Media/video.mp4"))
                              }
                          },
                          new Schedule
                          {
                              Cron = "* * * * *"
                          });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stop sending messages...");

        return Task.CompletedTask;
    }
}