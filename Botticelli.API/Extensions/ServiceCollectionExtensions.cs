namespace Botticelli.BotBase.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IHost UseBotticelli(this IHost host, IConfiguration config)
        {
            host.Services.AddHostedService<BotAdminConnectionService>();
            
            return host;
        }
    }
}
