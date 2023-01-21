using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;

namespace TelegramBotSample.MessageProcessors
{
    public class SampleMessageProcessor : IClientMessageProcessor
    {
        private IBot _bot;

        public async Task ProcessAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Message: \"{message.Body}\" processed!");

            if (message.Body.ToLower().Contains("removeit")) 
                await _bot.DeleteMessageAsync(new RemoveMessageRequest(message.Uid, message.ChatId), CancellationToken.None);

        }

        public void SetBot(IBot bot)
        {
            _bot = bot;
        }
    }
}
