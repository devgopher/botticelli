using Botticelli.Framework.Extensions.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Commands.Processors;

public class CommandChainProcessorBuilder<TInputCommand> where TInputCommand : class, ICommand
{
    private readonly IServiceCollection _services;
    private readonly List<Type> _typesChain = new(3);
    private ICommandChainProcessor<TInputCommand>? _chainProcessor;

    public CommandChainProcessorBuilder(IServiceCollection services)
    {
        _services = services;

        _typesChain.Add(typeof(CommandChainFirstElementProcessor<TInputCommand>));
        _services.AddSingleton<CommandChainFirstElementProcessor<TInputCommand>>();

        ProcessorFactoryBuilder.AddProcessor<CommandChainFirstElementProcessor<TInputCommand>>(_services);
    }

    public CommandChainProcessorBuilder<TInputCommand> AddNext<TNextProcessor>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TNextProcessor : class, ICommandChainProcessor<TInputCommand>
    {
        _typesChain.Add(typeof(TNextProcessor));
        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                _services.AddSingleton<TNextProcessor>();
                break;
            case ServiceLifetime.Scoped:
                _services.AddScoped<TNextProcessor>();
                break;
            case ServiceLifetime.Transient:
                _services.AddTransient<TNextProcessor>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
        } 
        
        ProcessorFactoryBuilder.AddProcessor<TNextProcessor>(_services);

        return this;
    }

    public ICommandChainProcessor<TInputCommand>? Build(IServiceProvider sp)
    {
        if (_typesChain.Count == 0) return null;
        var scope = sp.CreateScope();

        // initializing chain processors...

        _chainProcessor ??= sp.GetRequiredService(_typesChain.First()) as ICommandChainProcessor<TInputCommand>;

        // making a chain...
        var prev = _chainProcessor;

        foreach (var type in _typesChain.Skip(1))
        {
            var proc = scope.ServiceProvider.GetRequiredService(type) as ICommandChainProcessor<TInputCommand>;

            if (prev != null) prev.Next = proc;

            prev = proc;
        }

        return _chainProcessor;
    }
}