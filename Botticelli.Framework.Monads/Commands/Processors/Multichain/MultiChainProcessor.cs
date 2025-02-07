using Botticelli.Framework.Monads.Commands.Context;
using Botticelli.Interfaces;

namespace Botticelli.Framework.Monads.Commands.Processors.Multichain;

public abstract class MultiChainProcessor<TInChoise, TOutChoise>(IChoiseResolver ChoiseResolver)
    : IMultiChainProcessor<TInChoise, TOutChoise>
    where TOutChoise : IChoise, new()
    where TInChoise : IChoise
{
    public IBot? Bot { get; set; }

    public void SetBot(IBot bot) => Bot = bot;

    private IMultiChainProcessor<TOutChoise, IChoise> _next;

    public virtual async Task<TOutChoise> Process(TInChoise choise, CancellationToken token = default)
    {
        if (!ChoiseResolver.Resolve(choise))
            return new TOutChoise();

        return await InnerProcess(choise, token).ConfigureAwait(false);
    }

    public void SetNext<TNextOutChoise>(IMultiChainProcessor<TOutChoise, TNextOutChoise> next) where TNextOutChoise : IChoise 
        => _next = (IMultiChainProcessor<TOutChoise, IChoise>)next;

    public async Task<IChoise> RunNext(TOutChoise choise)
        => await _next.Process(choise);

    protected abstract Task<TOutChoise> InnerProcess(IChoise choise, CancellationToken token = default);
}