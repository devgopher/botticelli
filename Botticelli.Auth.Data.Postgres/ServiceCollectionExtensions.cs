using Botticelli.Auth.Extensions;
using Botticelli.Auth.Shared.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Auth.Data.Postgres;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a basic authorization implementation
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <param name="dbParameters"></param>
    /// <returns></returns>
    public static IServiceCollection AddPostgresBasicBotUserAuth(this IServiceCollection services, IConfiguration config, Action<DbContextOptionsBuilder> dbParameters)
    {
        services.AddBasicBotUserAuth();
        
        var settings = config.GetSection(nameof(AuthSettings))
                             .Get<AuthSettings>();
       
        return services.AddDbContext<AuthDefaultDbContext>(opt =>
                                            {
                                                dbParameters.Invoke(opt);
                                                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                                   .UseNpgsql(settings!.ConnectionString);
                                            },
                                            ServiceLifetime.Singleton);
    }
}