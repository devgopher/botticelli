using Botticelli.Framework.MessageProcessors;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Commands.Processors;

public class ClientProcessorFactory
{
    private static readonly IList<IClientMessageProcessor> ClientProcessors
            = new List<IClientMessageProcessor>(10);

    private readonly Random _rnd = new(DateTime.Now.Millisecond);

    public void AddProcessor<TProcessor, TBot>(IServiceProvider sp)
            where TProcessor : class, IClientMessageProcessor
            where TBot : IBot<TBot>
    {
        var procCnt = ClientProcessors.Count(x => x is TProcessor);

        if (procCnt > 10) return;

        var bot = sp.GetRequiredService<IBot<TBot>>();
        var proc = sp.CreateScope().ServiceProvider.GetRequiredService<TProcessor>();
        proc.AddBot(bot);
        proc.SetServiceProvider(sp);
        ClientProcessors.Add(proc);
    }

    public void AddChatMessageProcessor(IBot bot, IServiceProvider sp)
    {
        if (!ClientProcessors.Any(x => x is ChatMessageProcessor)) 
            ClientProcessors.Add(sp.GetRequiredService<ChatMessageProcessor>());
    }

    public IEnumerable<IClientMessageProcessor> GetProcessors()
        => ClientProcessors.AsEnumerable()
                           .OrderBy(_ => _rnd.Next() % ClientProcessors.Count)
                           .DistinctBy(p => p.GetType());
}