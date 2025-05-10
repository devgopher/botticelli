using Botticelli.Framework.Chained.Context;
using Botticelli.Framework.Chained.Monads.Commands;

namespace TelegramMonadsBasedBot.Commands;

public class MathCommand : IChainCommand
{
    public Guid Id { get; }
    public ICommandContext Context { get; init; }
}