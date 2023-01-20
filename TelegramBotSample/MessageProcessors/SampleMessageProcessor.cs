using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;

namespace TelegramBotSample.MessageProcessors
{
    public class SampleMessageProcessor : IClientMessageProcessor
    {
        public async Task ProcessAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Message: \"{message.Body}\" processed!");
        }
    }
}
