using Botticelli.AI.Message;
using Botticelli.AI.Settings;
using Botticelli.Bot.Interfaces.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.AI.AIProvider;

public abstract class GenericAiProvider : IAiProvider
{
    protected readonly IBotticelliBusClient _bus;
    protected readonly IHttpClientFactory _factory;
    protected readonly ILogger _logger;
    protected readonly IOptionsMonitor<AiSettings> _settings;

    public GenericAiProvider(IOptionsMonitor<AiSettings> settings,
                             IHttpClientFactory factory,
                             ILogger logger,
                             IBotticelliBusClient bus)
    {
        _settings = settings;
        _factory = factory;
        _logger = logger;
        _bus = bus;
    }

    public abstract Task SendAsync(AiMessage message, CancellationToken token);
    public abstract string AiName { get; }
}