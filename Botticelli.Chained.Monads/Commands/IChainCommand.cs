using Botticelli.Chained.Context;
using Botticelli.Framework.Commands;

namespace Botticelli.Chained.Monads.Commands;

public interface IChainCommand : ICommand
{
    public ICommandContext Context { get; init; }
}