using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;

namespace TelegramCommandChainSample.Commands.CommandProcessors;

public class SayHelloFinalCommandProcessor : CommandChainProcessor<GetNameCommand>
{
    public SayHelloFinalCommandProcessor(ILogger<CommandChainProcessor<GetNameCommand>> logger,
        ICommandValidator<GetNameCommand> validator, MetricsProcessor metricsProcessor) : base(logger, validator,
        metricsProcessor)
    {
    }

    public override async Task ProcessAsync(Message message, CancellationToken token)
    {
        message.Body = $"Have a nice day, dear {string.Join(' ', message.ProcessingArgs ?? new List<string>())}!";
        await Bot.SendMessageAsync(new SendMessageRequest
        {
            Message = message
        }, token);
    }

    protected override Task InnerProcess(Message message, string args, CancellationToken token) => Task.CompletedTask;
}