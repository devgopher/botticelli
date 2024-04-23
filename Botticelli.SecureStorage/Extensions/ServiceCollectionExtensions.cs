using Botticelli.SecureStorage.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.SecureStorage.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSecureStorage(this IServiceCollection services, string connectionString)
        {
            var secureStorageSettings = new SecureStorageSettings
            {
                ConnectionString = connectionString
            };

            return services.AddSingleton(secureStorageSettings)
                           .AddSingleton<SecureStorage>();
        }
    }
}