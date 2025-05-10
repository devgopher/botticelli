using Botticelli.Framework.Chained.Monads.Commands.Context;

namespace TelegramMonadsBasedBot.Commands;

public class MathCommand : IChainCommand
{
    public Guid Id { get; }
    public ICommandContext Context { get; init; }
}