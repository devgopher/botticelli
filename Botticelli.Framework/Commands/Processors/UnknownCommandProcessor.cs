using Botticelli.Framework.Commands.Validators;

namespace Botticelli.Framework.Commands.Processors
{
    public class UnknownCommandProcessor : CommandProcessor<Unknown>
    {
        public UnknownCommandProcessor(MessageTextManager messageTextManager,
            ITelegramBotClient botClient,
            IReadWriter<Chat, string> chatStorage,
            ILogger<UnknownCommandProcessor> logger,
            ICommandValidator<Unknown> validator) : base(messageTextManager, botClient, chatStorage, logger, validator)
        {
        }

        protected override Task InnerProcess(long chatId, params string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
