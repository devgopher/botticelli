using Botticelli.Framework.Chained.Context;
using Botticelli.Framework.Commands;

namespace Botticelli.Framework.Chained.Monads.Commands;

public interface IChainCommand : ICommand
{
    public ICommandContext Context { get; init; }
}