using Botticelli.Interfaces;

namespace Botticelli.Framework.Telegram.Handlers;

public static class ClientProcessorFactory
{
    private static readonly IList<IClientMessageProcessor> _clientProcessors
            = new List<IClientMessageProcessor>(10);

    public static void AddProcessor<TProcessor>()
            where TProcessor : class, IClientMessageProcessor, new()
    {
        var proc = _clientProcessors
                .FirstOrDefault(x => x is TProcessor);

        if (proc != default) 
            return;

        proc = new TProcessor();
        _clientProcessors.Add(proc);
    }

    public static IEnumerable<IClientMessageProcessor> GetProcessors()
        => _clientProcessors.AsEnumerable();
}