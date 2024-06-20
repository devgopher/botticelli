using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Commands.Processors;

public class CommandChainProcessorBuilder<TProcessor, TInputCommand>
        where TInputCommand : class, ICommand
        where TProcessor : ICommandChainProcessor<TInputCommand>, new()
{
    private readonly IServiceProvider _sp;
    private readonly TProcessor _chainProcessor;

    public CommandChainProcessorBuilder(IServiceProvider sp)
    {
        _sp = sp;
        _chainProcessor = new();
    }

    public CommandChainProcessorBuilder<TProcessor, TInputCommand> AddNext<TNextProcessor>()
            where TNextProcessor : ICommandChainProcessor
    {
        var next = _chainProcessor?.Next;

        while (next != null)
        {
            var prev = next;
            next = _sp.GetRequiredService<TNextProcessor>();
            prev.Next = next;
        }

        return this;
    }
    
    
    public TProcessor Build() => _chainProcessor;
}