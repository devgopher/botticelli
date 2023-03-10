using Botticelli.Framework.Telegram;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;

namespace TelegramBotSample;

public class TestBotHostedService : IHostedService
{
    private readonly IBot<TelegramBot> _telegramBot;


    public TestBotHostedService(IBot<TelegramBot> telegramBot) => _telegramBot = telegramBot;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        //return Task.Factory.StartNew(async ()
        //                                     =>
        //                             {
        //                                 while (!cancellationToken.IsCancellationRequested)
        //                                 {
        //                                     Console.WriteLine("Start sending messages...");
        //                                     await SendTestMessage();

        //                                     Thread.Sleep(30000);
        //                                 }
        //                             },
        //                             cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stop sending messages...");

        return Task.CompletedTask;
    }

    private async Task SendTestMessage()
    {
        var msg = new Message("testid")
        {
            Body = "testmsg",
            Subject = "subj",
            ChatId = "-844372214", // "156620699",
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
        };

        //Console.WriteLine($"Sending: {JsonConvert.SerializeObject(msg)}...");

        var req = SendMessageRequest.GetInstance();
        req.Message = msg;

        await SendTelegramMessage(req);
    }

    private async Task SendTelegramMessage(SendMessageRequest req)
    {
        var sentResponse = await _telegramBot.SendMessageAsync(req, CancellationToken.None);

        Console.WriteLine($"msg sent: {sentResponse.MessageSentStatus}");

        if (sentResponse.MessageSentStatus == MessageSentStatus.Fail) return;
    }
}