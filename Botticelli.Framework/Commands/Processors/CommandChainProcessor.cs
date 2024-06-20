using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

public abstract class CommandChainProcessor<TCommand> : CommandProcessor<TCommand> 
        where TCommand : class, ICommand
{
    protected CommandChainProcessor(ILogger logger, 
                                    ICommandValidator<TCommand> validator,
                                    MetricsProcessor metricsProcessor) 
            : base(logger, validator, metricsProcessor)
    {
    }

    protected ICommandProcessor Next { get; set; }
    public override Task ProcessAsync(Message message, CancellationToken token)
    {
        var processTask = base.ProcessAsync(message, token);

        return Next == default ? processTask : processTask.ContinueWith(task => Next.ProcessAsync(message, token), token);
    }
}