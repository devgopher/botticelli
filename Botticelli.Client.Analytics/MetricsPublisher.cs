using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Client.Analytics.Settings;
using Flurl;
using Flurl.Http;

namespace Botticelli.Client.Analytics;

public class MetricsPublisher
{
    private readonly AnalyticsSettings _settings;

    public MetricsPublisher(AnalyticsSettings settings)
    {
        _settings = settings;
    }

    public async Task Publish(IMetricObject metric, CancellationToken token)
        => await Url.Combine(_settings.TargetUrl, "/metrics/receiver/ReceiveMetric")
            .SetQueryParams(metric)
            .SendAsync(HttpMethod.Get, cancellationToken: token); // polly!
}