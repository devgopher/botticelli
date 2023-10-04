using Botticelli.Server.Analytics.Requests;

namespace Botticelli.Server.Analytics.Services;

public class MetricsInputService : IMetricsInputService
{
    private readonly MetricsReaderWriter _rw;

    public MetricsInputService(MetricsReaderWriter rw) => _rw = rw;

    public async Task PushMetricAsync(PushMetricRequest request, CancellationToken token)
    {
        await _rw.WriteAsync(request, token);
    }
}