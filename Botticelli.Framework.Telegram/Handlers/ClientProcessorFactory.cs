using Botticelli.Interfaces;

namespace Botticelli.Framework.Telegram.Handlers;

public static class ClientProcessorFactory
{
    private static readonly IList<IClientMessageProcessor> ClientProcessors
            = new List<IClientMessageProcessor>(10);

    public static void AddProcessor<TProcessor>(IBot bot, IServiceProvider sp)
            where TProcessor : class, IClientMessageProcessor, new()
    {
        var proc = ClientProcessors
                .FirstOrDefault(x => x is TProcessor);

        if (proc != default) return;

        proc = new TProcessor();
        proc.SetBot(bot);
        proc.SetServiceProvider(sp);
        ClientProcessors.Add(proc);
    }

    public static IEnumerable<IClientMessageProcessor> GetProcessors()
        => ClientProcessors.AsEnumerable();
}