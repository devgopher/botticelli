using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Interfaces;

namespace Botticelli.Framework.Extensions.Processors;

public class ProcessorFactory(IEnumerable<ICommandProcessor> processors)
{
    public IEnumerable<IClientMessageProcessor> GetProcessors(bool excludeChain = true)
    {
        return processors.AsEnumerable()
                         .Where(p => !excludeChain || !p.GetType().IsAssignableTo(typeof(ICommandChainProcessor)))
                         .DistinctBy(p => p.GetType());
    }

    public IEnumerable<ICommandChainProcessor> GetCommandChainProcessors()
    {
        return processors.AsEnumerable()
                         .Where(p => p.GetType().IsAssignableTo(typeof(ICommandChainFirstElementProcessor)))
                         .Cast<ICommandChainFirstElementProcessor>()
                         .DistinctBy(p => p.GetType());
    }
}