using Botticelli.Framework.MessageProcessors;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Commands.Processors;

public class ClientProcessorFactory
{
    private static readonly IList<IClientMessageProcessor> _clientProcessors
            = new List<IClientMessageProcessor>(10);

    private Random rnd = new Random(DateTime.Now.Millisecond);

    public void AddProcessor<TProcessor, TBot>(IServiceProvider sp)
            where TProcessor : class, IClientMessageProcessor
            where TBot : IBot
    {
        var procCnt = _clientProcessors.Count(x => x is TProcessor);

        if (procCnt > 10)
            return;

        var bot = sp.GetRequiredService<TBot>();
        var proc = sp.CreateScope().ServiceProvider.GetRequiredService<TProcessor>();
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
        => _clientProcessors.AsEnumerable()
                            .OrderBy(_ => rnd.Next() % _clientProcessors.Count)
                            .DistinctBy(p => p.GetType());
}