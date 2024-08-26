using System.Collections.Concurrent;
using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Interfaces;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

/// <summary>
/// A command chain processor means, that we've a command on input and several step to process it
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

    public HashSet<Guid> ChainIds { get; } = new(100);
    public ICommandChainProcessor Next { get; set; }
}