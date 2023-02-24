using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Telegram;
using Botticelli.Interfaces;
using TelegramBotSample.Commands;

namespace TelegramBotSample;

public class SampleCommandProcessor : CommandProcessor<SampleCommand>
{
    public SampleCommandProcessor(IBot<TelegramBot> botClient, ILogger<SampleCommandProcessor> logger, ICommandValidator<SampleCommand> validator) : base(botClient, logger, validator)
    {
    }

    protected override async Task InnerProcess(long chatId, CancellationToken token, params string[] args)
        => Console.WriteLine("Command received!!");
}

