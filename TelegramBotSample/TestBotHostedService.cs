using Botticelli.Framework.Telegram;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.Utils;
using Botticelli.Shared.ValueObjects;
using Newtonsoft.Json;

namespace TelegramBotSample
{
    public class TestBotHostedService : IHostedService
    {
        private readonly IBot<TelegramBot> _bot;

        public TestBotHostedService(IBot<TelegramBot> bot) => _bot = bot;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"Start sending messages...");
                await SendTestMessage();
                Thread.Sleep(20000);
            }
        }

        private async Task SendTestMessage()
        {
            var msg = new Message("testid")
            {
                Body = "testmsg",
                Subject = "subj",
                ChatId = "156620699",
                Attachments = new List<BinaryAttachment>()
                {
                    new BinaryAttachment(Guid.NewGuid().ToString(), "testpic.png", MediaType.Image,
                        File.ReadAllBytes($"Media/testpic.png")),
                    new BinaryAttachment(Guid.NewGuid().ToString(), "voice.mp3", MediaType.Voice,
                        File.ReadAllBytes($"Media/voice.mp3")),
                    new BinaryAttachment(Guid.NewGuid().ToString(), "video.mp4", MediaType.Video,
                        File.ReadAllBytes($"Media/video.mp4")),
                }
            };

            Console.WriteLine($"Sending: {JsonConvert.SerializeObject(msg)}...");

            var req = SendMessageRequest.GetInstance();
            req.Message = msg;

            var sentResponse = await _bot.SendAsync(req, CancellationToken.None);

            Console.WriteLine($"msg sent: {sentResponse.MessageSentStatus}");

            if (sentResponse.MessageSentStatus == MessageSentStatus.FAIL)
                return;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Stop sending messages...");
        }
    }
}
