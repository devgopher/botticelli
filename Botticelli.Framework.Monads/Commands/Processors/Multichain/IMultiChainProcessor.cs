using Botticelli.Interfaces;

namespace Botticelli.Framework.Monads.Commands.Processors.Multichain;

/// <summary>
///     Chain processor with multipath processing
/// </summary>
/// <typeparam name="TOutChoise">Output choice type</typeparam>
/// <typeparam name="TInChoise">Input choise type</typeparam>
public interface IMultiChainProcessor<in TInChoise, TOutChoise>
where TOutChoise : IChoise
where TInChoise : IChoise
{
    public IBot? Bot { get; }

    public void SetBot(IBot bot);

    public Task<TOutChoise> Process(TInChoise choice, CancellationToken token = default);
    
    public void SetNext<TNextOutChoise>(IMultiChainProcessor<TOutChoise, TNextOutChoise> next) 
        where TNextOutChoise : IChoise;

    public Task<IChoise> RunNext(TOutChoise choise);
}