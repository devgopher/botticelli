using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Client.Analytics.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMetrics(this IServiceCollection services)
        {
            return services.AddScoped<MetricsPublisher>();
        }
    }
}
