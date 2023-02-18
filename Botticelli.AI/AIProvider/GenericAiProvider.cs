using Botticelli.AI.Extensions;
using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Shared.API.Client.Requests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Botticelli.AI.AIProvider;

public abstract class GenericAiProvider : IAiProvider
{
    private readonly IBotticelliBusAgent _bus;
    private readonly IHttpClientFactory _factory;
    private readonly ILogger _logger;
    private readonly IOptionsMonitor<AISettings> _settings;

    public GenericAiProvider(IOptionsMonitor<AISettings> settings,
                             IHttpClientFactory factory,
                             ILogger logger,
                             IBotticelliBusAgent bus)
    {
        _settings = settings;
        _factory = factory;
        _logger = logger;
        _bus = bus;
    }

    public abstract Task SendAsync(AIMessage message, CancellationToken token);

    public virtual async Task ProcessResponses(CancellationToken token)
    {
        _logger.LogInformation($"{nameof(ProcessResponses)}() started...");

        await Task.Run(async () =>
                       {
                           while (!token.IsCancellationRequested)
                           {
                               var response = await InnerReceiveResponse();

                               if (response != default)
                                       // TODO: reliability!!
                                   await _bus.SendResponse(new SendMessageRequest(Guid.NewGuid().ToString())
                                                           {
                                                               Message = response
                                                           },
                                                           token);
                               //_logger.LogTrace($"Got response from a bot!");
                               //if (botResponse.MessageSentStatus != MessageSentStatus.OK)
                               //    _logger.LogError($"Bot response is: {botResponse.MessageSentStatus}, " +
                               //                     $"{botResponse.TechMessage}. It's not successful!");
                               //throw new AiException($"Bot response is: {botResponse.MessageSentStatus}, " +
                               //                      $"{botResponse.TechMessage}. It's not successful!")
                               Thread.Sleep(200);
                           }
                       },
                       token);

        _logger.LogInformation($"{nameof(ProcessResponses)}() stopped");
    }

    protected abstract Task<AIMessage?> InnerReceiveResponse();
}