using Botticelli.Auth.Extensions;
using Botticelli.Auth.Shared.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Auth.Data.Sqlite;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a basic authorization implementation
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <param name="dbParameters"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqliteBasicBotUserAuth(this IServiceCollection services, IConfiguration config,
        Action<DbContextOptionsBuilder>? dbParameters = null)
    {
        services.AddBasicBotUserAuth();

        var settings = config.GetSection(nameof(AuthSettings))
            .Get<AuthSettings>();

        return services.AddDbContext<AuthDefaultDbContext>(opt =>
            {
                dbParameters?.Invoke(opt);
                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseSqlite(settings!.ConnectionString, b => b.MigrationsAssembly("Botticelli.Auth.Data.Sqlite"));
            },
            ServiceLifetime.Singleton);
    }
}