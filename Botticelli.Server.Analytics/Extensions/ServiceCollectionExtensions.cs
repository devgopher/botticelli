using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Server.Analytics.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMetrics(this IServiceCollection services)
        {
            return services.AddScoped<MetricsWriter>();
        }
    }
}
