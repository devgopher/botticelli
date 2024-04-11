using Botticelli.Framework.Commands.Processors;
using Deveel.Webhooks;
using Deveel.Webhooks.Facebook;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Facebook.Messages.WebHooksReceivers;


public class FacebookHandler : IWebhookHandler<FacebookWebhook>
{
    private readonly ILogger<FacebookHandler> _logger;
    private readonly ClientProcessorFactory _processorFactory;

    public FacebookHandler(ILogger<FacebookHandler> logger, ClientProcessorFactory processorFactory)
    {
        _logger = logger;
        _processorFactory = processorFactory;
    }

    public async Task HandleAsync(FacebookWebhook webhook,
        CancellationToken cancellationToken = new())
    {
        _logger.LogInformation($"{nameof(HandleAsync)}() started for a hook");
        try
        {
            var messages = webhook.Entries
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(HandleAsync)}() error", ex);
        }
    }
}