using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Interfaces;
using TelegramBotSample.Commands;

namespace TelegramBotSample
{
    public class SampleCommandProcessor : CommandProcessor<SampleCommand>
    {
        protected override async Task InnerProcess(long chatId, CancellationToken token, params string[] args) 
            => Console.WriteLine($"Command received!!");

        public SampleCommandProcessor(IBot botClient, ILogger logger, ICommandValidator<SampleCommand> validator) : base(botClient, logger, validator)
        {
        }
    }
}
