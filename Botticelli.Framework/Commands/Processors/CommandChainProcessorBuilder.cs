using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Commands.Processors;

public class CommandChainProcessorBuilder<TInputCommand>(IServiceCollection services)
        where TInputCommand : class, ICommand
{
    private ICommandChainProcessor<TInputCommand> _chainProcessor;
    private readonly List<Type> _typesChain = new(2);

    public CommandChainProcessorBuilder<TInputCommand> AddNext<TNextProcessor>()
            where TNextProcessor : class, ICommandChainProcessor<TInputCommand>
    {
        _typesChain.Add(typeof(TNextProcessor));

        return this;
    }
    
    public ICommandChainProcessor<TInputCommand> Build()
    {
        if (!_typesChain.Any()) return null;
        
        // initializing chain processors...
        foreach (var type in _typesChain) services.AddScoped(type);
        
        var sp = services.BuildServiceProvider();
        
        _chainProcessor ??= sp.GetRequiredService(_typesChain.First()) as ICommandChainProcessor<TInputCommand>;

        // making a chain...
        var prev = _chainProcessor;
        foreach (var type in _typesChain.Skip(1))
        {
            var proc = sp.GetRequiredService(type) as ICommandChainProcessor<TInputCommand>;
            
            if (_chainProcessor is {Next: null})
            {
                _chainProcessor.Next = proc;
                continue;
            }

            prev.Next = proc;

            prev = proc;
        }

        return _chainProcessor;
    }
}