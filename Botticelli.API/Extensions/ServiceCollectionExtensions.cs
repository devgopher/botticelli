namespace Botticelli.BotBase.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseBotticelli(this IServiceCollection services, IConfiguration config)
        {
            services.AddHostedService<BotAdminConnectionService>();
            
            return services;
        }
    }
}
