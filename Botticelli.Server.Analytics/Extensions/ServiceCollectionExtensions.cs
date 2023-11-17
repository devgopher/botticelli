using Botticelli.Server.Analytics.Cache;
using Botticelli.Server.Analytics.Services;
using Botticelli.Server.Analytics.Settings;

namespace Botticelli.Server.Analytics.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMetrics(this IServiceCollection services, AnalyticsSettings settings) =>
        services.AddScoped<MetricsReaderWriter>()
            .AddScoped<IMetricsInputService, MetricsInputService>()
            .AddScoped<IMetricsOutputService, MetricsOutputService>()
            .AddScoped<ICacheAccessor, CacheAccessor>(_ => new CacheAccessor(settings.MaxCacheSize));
}