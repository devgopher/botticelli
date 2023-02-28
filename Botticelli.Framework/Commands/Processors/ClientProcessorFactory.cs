using Botticelli.Framework.MessageProcessors;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Commands.Processors;

public class ClientProcessorFactory
{
    private static readonly IList<IClientMessageProcessor> _clientProcessors
            = new List<IClientMessageProcessor>(10);

    public void AddProcessor<TProcessor, TBot>(IServiceProvider sp)
            where TProcessor : class, IClientMessageProcessor
            where TBot : IBot
    {
        var proc = _clientProcessors
                .FirstOrDefault(x => x is TProcessor);

        if (proc != default) return;

        var bot = sp.GetRequiredService<TBot>();
        proc = sp.GetRequiredService<TProcessor>();
        proc.SetBot(bot);
        proc.SetServiceProvider(sp);
        _clientProcessors.Add(proc);
    }

    public void AddChatMessageProcessor(IBot bot, IServiceProvider sp)
    {
        if (!_clientProcessors.Any(x => x is ChatMessageProcessor))
            _clientProcessors.Add(sp.GetRequiredService<ChatMessageProcessor>());
    }

    public IEnumerable<IClientMessageProcessor> GetProcessors()
        => _clientProcessors.AsEnumerable();
}