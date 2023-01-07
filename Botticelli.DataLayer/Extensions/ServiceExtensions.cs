using Botticelli.DataLayer.Context;
using Botticelli.DataLayer.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.DataLayer.Extensions
{
    /// <summary>
    /// Service extensions to add a storage for chats
    /// </summary>
    public static class ServiceExtensions
    {
        public static IServiceCollection AddChatStorage(this IServiceCollection services,
            Action<DbContextOptionsBuilder> dbFunc,
            IConfiguration config)
            => AddStorage<ChatContext>(services, dbFunc, config);

        public static IServiceCollection AddStorage<TContext>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> dbFunc,
            IConfiguration config)
        where TContext : DbContext
        {
            var settings = config.Get<StorageSettings>();
            services.AddDbContext<TContext>(o => dbFunc(o));

            return services;
        }
    }
}
