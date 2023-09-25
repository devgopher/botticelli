using MediatR;
using Microsoft.Extensions.Logging;

namespace Botticelli.Client.Analytics.Handlers;

public abstract class BasicHandler<TArgs> : IRequestHandler<TArgs> where TArgs : IRequest
{
    protected readonly MetricsPublisher _publisher;
    protected readonly ILogger _logger;

    public BasicHandler(MetricsPublisher publisher, ILogger logger)
    {
        _publisher = publisher;
        _logger = logger;
    }
    
    public virtual async Task Handle(TArgs request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogTrace($"Metric {request.GetType().Name} handling...");
            await _publisher.Publish(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }
    }
}