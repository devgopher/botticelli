using Botticelli.Chained.Monads.Commands.Result;
using Microsoft.Extensions.Logging;

namespace Botticelli.Chained.Monads.Commands.Processors;

public class InputCommandProcessor<TCommand>(ILogger<InputCommandProcessor<TCommand>> logger)
        : ChainProcessor<TCommand>(logger)
        where TCommand : IChainCommand
{
    protected override Task InnerProcessAsync(IResult<TCommand> stepResult, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerErrorProcessAsync(FailResult<TCommand> stepResult, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}