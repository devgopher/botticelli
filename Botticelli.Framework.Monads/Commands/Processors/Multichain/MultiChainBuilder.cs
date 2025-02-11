using Botticelli.Framework.Monads.Commands.Context;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Monads.Commands.Processors.Multichain;

public class MultiChainBuilder<TCommand>(IServiceCollection services)
    where TCommand : IChainCommand
{
    private IMultiChainProcessor<IChoise, IChoise>? _element;
    private IBot? _bot;
    private MultiChainRunner<TCommand>? _runner;

    public MultiChainBuilder<TCommand> Next<TInChoise, TOutChoise>(IMultiChainProcessor<TInChoise, TOutChoise> processor) 
        where TInChoise : IChoise
        where TOutChoise : IChoise
    {
        if (_element == null)
        {
            _element = (IMultiChainProcessor<IChoise, IChoise>?)processor;

            return this;
        }

        _element.SetNext((IMultiChainProcessor<IChoise, TOutChoise>)processor);
        _element = (IMultiChainProcessor<IChoise, IChoise>)processor;

        return this;
    }

    public MultiChainBuilder<TCommand> Next<TProcessor, TInChoise, TOutChoise>()
        where TProcessor : class, IMultiChainProcessor<TInChoise, TOutChoise>
        where TInChoise : IChoise 
        where TOutChoise : IChoise
    {
        services.AddScoped<TProcessor>();
        var processor = services.BuildServiceProvider()
            .GetRequiredService<TProcessor>();

        return Next(processor);
    }

    public MultiChainBuilder<TCommand> Next<TProcessor, TInChoise, TOutChoise>(Action<TProcessor> func)
        where TProcessor : class, IMultiChainProcessor<TInChoise, TOutChoise>
        where TInChoise : IChoise 
        where TOutChoise : IChoise
    {
        services.AddScoped<TProcessor>();
        var processor = services.BuildServiceProvider()
            .GetRequiredService<TProcessor>();
        func(processor);

        return Next(processor);
    }

    public MultiChainBuilder<TCommand> SetBot<TBot>(TBot bot)
        where TBot : IBot<TBot>
    {
        _bot = bot;

        return this;
    }

    public MultiChainRunner<TCommand> Build()
    {
        var sp = services.BuildServiceProvider();
        _bot ??= sp.GetServices<IBot>().FirstOrDefault();

        if (_bot == default)
            throw new NullReferenceException($"Bot should be set up: call {nameof(SetBot)} to set a bot instance!");

        foreach (var processor in _element) processor.SetBot(_bot);

        _runner ??= new MultiChainRunner<TCommand>(_element, sp
            .GetRequiredService<ILogger<MultiChainRunner<TCommand>>>());

        return _runner;
    }
}