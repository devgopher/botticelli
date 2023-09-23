using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Client.Analytics.Settings;
using Flurl.Http;
using MediatR;

namespace Botticelli.Client.Analytics
{
    public class MetricsPublisher
    {
        private readonly AnalyticsSettings _settings;
        public MetricsPublisher(AnalyticsSettings settings)
        {
            _settings = settings;
        }

        public async Task Publish<TObject>(TObject args, CancellationToken token)
        {
            var metricsObject = new MetricObject<TObject>(args);
            
            await _settings.TargetUrl.SendJsonAsync(HttpMethod.Post, metricsObject, token); // polly!

        }
    }
}