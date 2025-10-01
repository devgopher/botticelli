using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Extensions.Processors;

public static class ProcessorFactoryBuilder
{
    private static IServiceCollection? _serviceCollection;
    private static readonly List<Type> ProcessorTypes = [];

    public static void AddProcessor<TProcessor>(IServiceCollection serviceCollection)
            where TProcessor : class, ICommandProcessor
    {
        _serviceCollection ??= serviceCollection;
        ProcessorTypes.Add(typeof(TProcessor));
    }

    public static ProcessorFactory Build(IServiceProvider sp)
    {
        if (_serviceCollection == null) 
            return new ProcessorFactory([]);

        var processors = ProcessorTypes
                         .Select(pt =>
                         {
                             var processor = sp.GetRequiredService(pt) as ICommandProcessor;
                             processor?.SetBot(sp.GetRequiredService<IBot>());
                             processor?.SetServiceProvider(sp);

                             return processor;
                         })
                         .ToArray();

        return new ProcessorFactory(processors!);
    }
}