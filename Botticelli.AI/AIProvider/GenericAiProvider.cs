using Botticelli.AI.Message;
using Botticelli.AI.Settings;
using Botticelli.Bot.Interfaces.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.AI.AIProvider;

public abstract class GenericAiProvider : IAiProvider
{
    protected readonly IBusClient Bus;
    protected readonly IHttpClientFactory Factory;
    protected readonly ILogger Logger;
    protected readonly IOptionsMonitor<AiSettings> Settings;

    public GenericAiProvider(IOptionsMonitor<AiSettings> settings,
                             IHttpClientFactory factory,
                             ILogger logger,
                             IBusClient bus)
    {
        Settings = settings;
        Factory = factory;
        Logger = logger;
        Bus = bus;
    }

    public abstract Task SendAsync(AiMessage message, CancellationToken token);
    public abstract string AiName { get; }
}