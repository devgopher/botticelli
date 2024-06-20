using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

/// <summary>
/// A command chain processor means, that we've a command on on input and several step to process it
/// During a processing every processor changes values in 
/// </summary>
/// <typeparam name="TInputCommand"></typeparam>
public abstract class CommandChainProcessor<TInputCommand> : CommandProcessor<TInputCommand>, ICommandChainProcessor<TInputCommand>
        where TInputCommand : class, ICommand
{
    public CommandChainProcessor(ILogger<CommandChainProcessor<TInputCommand>> logger,
                                 ICommandValidator<TInputCommand> validator,
                                 MetricsProcessor metricsProcessor)
            : base(logger, validator, metricsProcessor)
    {
    }

    public ICommandChainProcessor Next { get; set; }
    
    public override Task ProcessAsync(Message message, CancellationToken token)
    {
        var processTask = base.ProcessAsync(message, token);

        _logger.LogDebug(Next == default ?
                                 $"{nameof(CommandChainProcessor<TInputCommand>)} : no next step, returning" :
                                 $"{nameof(CommandChainProcessor<TInputCommand>)} : next step is '{Next?.GetType().Name}'");

        return Next == default ? processTask : processTask.ContinueWith(task => Next.ProcessAsync(message, token), token);
    }
}