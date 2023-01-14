using Botticelli.BotBase.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.BotBase.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseBotticelli(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<BotStatusService>();
            //services.AddScoped<IBotControllerService, >();

            services.AddHostedService<BotStatusService>();
            services.AddSingleton<ServerSettings>();

            return services;
        }
    }
}
