using Botticelli.Chained.Context;
using Botticelli.Chained.Monads.Commands;

namespace TelegramMonadsBasedBot.Commands;

public class MathCommand : IChainCommand
{
    public Guid Id { get; }
    public ICommandContext Context { get; init; }
}