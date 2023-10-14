using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Client.Analytics.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMetrics(this IServiceCollection services)
        {
            return services.AddScoped<MetricsPublisher>()
                .AddMediatR(c => c.RegisterServicesFromAssembly(typeof(MetricsPublisher).Assembly))
                .AddScoped<MetricsProcessor>();
        }
    }
}
