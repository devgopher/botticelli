using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;

namespace TelegramCommandChainSample.Commands.CommandProcessors;

public class SayHelloFinalCommandProcessor : CommandChainProcessor<SayHelloCommand>
{
    public SayHelloFinalCommandProcessor(ILogger<CommandChainProcessor<SayHelloCommand>> logger, ICommandValidator<SayHelloCommand> validator, MetricsProcessor metricsProcessor) : base(logger, validator, metricsProcessor)
    {
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        message.Body = $"Have a nice day, dear {message.Body}!";
        await Bot.SendMessageAsync(new SendMessageRequest
        {
            Message = message
        }, token);
    }
}