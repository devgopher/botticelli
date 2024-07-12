using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;

namespace TelegramCommandChainSample.Commands.CommandProcessors;

public class SayHelloCommandProcessor : CommandChainProcessor<SayHelloCommand>
{
    public SayHelloCommandProcessor(ILogger<CommandChainProcessor<SayHelloCommand>> logger, ICommandValidator<SayHelloCommand> validator, MetricsProcessor metricsProcessor) : base(logger, validator, metricsProcessor)
    {
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        message.Body = "What's your name?";
        await Bot.SendMessageAsync(new SendMessageRequest
        {
            Message = message
        }, token);
    }
}