using Botticelli.DataLayer.Context;
using Botticelli.DataLayer.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.DataLayer.Extensions;

/// <summary>
///     Service extensions to add a storage for chats
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    ///     Adds a storage for chats
    /// </summary>
    /// <param name="services"></param>
    /// <param name="dbFunc"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddChatStorage(this IServiceCollection services,
                                                    Action<DbContextOptionsBuilder, StorageSettings> dbFunc,
                                                    IConfiguration config) =>
            AddStorage<BotticelliContext>(services, dbFunc, config);

    /// <summary>
    ///     Adds a storage for messages
    /// </summary>
    /// <param name="services"></param>
    /// <param name="dbFunc"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddMessageStorage(this IServiceCollection services,
                                                       Action<DbContextOptionsBuilder, StorageSettings> dbFunc,
                                                       IConfiguration config) =>
            AddStorage<BotticelliContext>(services, dbFunc, config);

    /// <summary>
    ///     Adds a new storage
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="dbFunc"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddStorage<TContext>(this IServiceCollection services,
                                                          Action<DbContextOptionsBuilder, StorageSettings> dbFunc,
                                                          IConfiguration config)
            where TContext : DbContext
    {
        var settings = config.Get<StorageSettings>();
        services.AddDbContext<TContext>(o => dbFunc(o, settings));

        return services;
    }
}