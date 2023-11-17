using Botticelli.Server.Analytics.Services;

namespace Botticelli.Server.Analytics.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMetrics(this IServiceCollection services) =>
        services.AddScoped<MetricsReaderWriter>()
            .AddScoped<IMetricsInputService, MetricsInputService>()
            .AddScoped<IMetricsOutputService, MetricsOutputService>();
}