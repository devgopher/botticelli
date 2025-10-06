using Botticelli.Framework.Commands;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.Chained.Monads.Commands.Result;

/// <summary>
///     Fail result
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public class FailResult<TCommand> : BasicResult<TCommand> where TCommand : ICommand
{
    private FailResult(TCommand command, string techMessage, Message? message) : base(command, false)
    {
    }

    public string? TechMessage { get; init; }

    public static FailResult<TCommand> Create(TCommand command,
                                              string techMessage,
                                              Message? message = null)
    {
        return new FailResult<TCommand>(command, techMessage, message);
    }
}