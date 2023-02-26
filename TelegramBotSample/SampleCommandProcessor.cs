using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Telegram;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using TelegramBotSample.Commands;

namespace TelegramBotSample;

public class SampleCommandProcessor : CommandProcessor<SampleCommand>
{
    public SampleCommandProcessor(IBot<TelegramBot> bot, ILogger<SampleCommandProcessor> logger, ICommandValidator<SampleCommand> validator) : base(bot, logger, validator)
    {
    }

    protected override async Task InnerProcess(Message message, CancellationToken token)
        => Console.WriteLine("Command received!!");
}