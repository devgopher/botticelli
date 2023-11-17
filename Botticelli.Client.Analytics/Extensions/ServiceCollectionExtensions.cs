using Botticelli.Client.Analytics.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Client.Analytics.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMetrics(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(AnalyticsSettings)).Get<AnalyticsSettings>();
        return services.AddSingleton<MetricsPublisher>()
            .AddSingleton(settings)
            .AddMediatR(c => c.RegisterServicesFromAssembly(typeof(MetricsPublisher).Assembly))
            .AddSingleton<MetricsProcessor>();
    }
}