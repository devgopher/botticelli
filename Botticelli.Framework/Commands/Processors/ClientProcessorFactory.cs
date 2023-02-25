using Botticelli.Framework.MessageProcessors;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

public static class ClientProcessorFactory
{
    private static readonly IList<IClientMessageProcessor> ClientProcessors
            = new List<IClientMessageProcessor>(10);

    public static void AddProcessor<TProcessor, TBot>(IServiceProvider sp)
            where TProcessor : class, IClientMessageProcessor, new()
            where TBot : IBot
    {
        var proc = ClientProcessors
                .FirstOrDefault(x => x is TProcessor);

        if (proc != default) return;

        var bot = sp.GetRequiredService<TBot>();
        proc = new TProcessor();
        proc.SetBot(bot);
        proc.SetServiceProvider(sp);
        ClientProcessors.Add(proc);
    }

    public static void AddChatMessageProcessor(IBot bot, IServiceProvider sp)
    {
        var proc = ClientProcessors
                .FirstOrDefault(x => x is ChatMessageProcessor);

        if (proc != default) return;

        proc = new ChatMessageProcessor(sp.GetRequiredService<ILogger<ChatMessageProcessor>>(), 
                                        sp.GetRequiredService<CommandProcessorFactory>());
        ClientProcessors.Add(proc);
    }

    public static IEnumerable<IClientMessageProcessor> GetProcessors()
        => ClientProcessors.AsEnumerable();
}