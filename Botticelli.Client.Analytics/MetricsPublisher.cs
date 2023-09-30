using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Client.Analytics.Settings;
using Flurl.Http;

namespace Botticelli.Client.Analytics
{
    public class MetricsPublisher
    {
        private readonly AnalyticsSettings _settings;
        public MetricsPublisher(AnalyticsSettings settings) => _settings = settings;

        public async Task Publish(MetricObject metric, CancellationToken token) 
            => await _settings.TargetUrl.SendJsonAsync(HttpMethod.Post, metric, token); // polly!
    }
}