using Botticelli.Framework.Telegram;
using Botticelli.Framework.Viber;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using Newtonsoft.Json;

namespace TelegramBotSample
{
    public class TestBotHostedService : IHostedService
    {
        private readonly IBot<TelegramBot> _telegramBot;

        //private readonly IBot<ViberBot> _viberBot;

        public TestBotHostedService(IBot<TelegramBot> telegramBot/*, IBot<ViberBot> viberBot*/)
        {
            _telegramBot = telegramBot;
            //_viberBot = viberBot;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async ()
                =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Start sending messages...");
                    await SendTestMessage();

                    Thread.Sleep(3000);
                }
            }, cancellationToken);
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
                    new(Guid.NewGuid().ToString(), "testpic.png", MediaType.Image,
                        File.ReadAllBytes($"Media/testpic.png")),
                    new(Guid.NewGuid().ToString(), "voice.mp3", MediaType.Voice,
                        File.ReadAllBytes($"Media/voice.mp3")),
                    new(Guid.NewGuid().ToString(), "video.mp4", MediaType.Video,
                        File.ReadAllBytes($"Media/video.mp4")),
                }
            };

                //Console.WriteLine($"Sending: {JsonConvert.SerializeObject(msg)}...");

            var req = SendMessageRequest.GetInstance();
            req.Message = msg;

            await SendTelegramMessage(req);
        }

        private async Task SendTelegramMessage(SendMessageRequest req)
        {
            var sentResponse = await _telegramBot.SendAsync(req, CancellationToken.None);

            Console.WriteLine($"msg sent: {sentResponse.MessageSentStatus}");

            if (sentResponse.MessageSentStatus == MessageSentStatus.FAIL)
                return;
        }

        //private async Task SendViberMessage(SendMessageRequest req)
        //{
        //    var sentResponse = await _viberBot.SendAsync(req, CancellationToken.None);

        //    Console.WriteLine($"msg sent: {sentResponse.MessageSentStatus}");

        //    if (sentResponse.MessageSentStatus == MessageSentStatus.FAIL)
        //        return;
        //}


        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Stop sending messages...");

            return Task.CompletedTask;
        }
    }
}
