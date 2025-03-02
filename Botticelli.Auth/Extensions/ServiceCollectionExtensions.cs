using Botticelli.Auth.Data;
using Botticelli.Auth.Dto.Credentials;
using Botticelli.Auth.Dto.User;
using Botticelli.Auth.Mapping;
using Botticelli.Auth.Rules;
using Botticelli.Auth.Services;
using Botticelli.Auth.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Auth.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a default authorization implementation
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultBotUserAuth(this IServiceCollection services, IConfiguration config)
    {
        MappingProfile.UseProfile();
        
        var settings = config.GetSection(nameof(AuthSettings))
                             .Get<AuthSettings>();
        
        services.AddSingleton<IIdentifier<BotAuthCredentials, BotUserInfo>, DefaultBotAuthService>()
                .AddSingleton<IManager<BotUserInfo>, DefaultUserManager>()
                .AddSingleton<IManager<BotUserRoleInfo>, DefaultUserRoleManager>()
                .AddSingleton<CredentialsRulesBuilder<BotAuthCredentials>>()
                .AddDbContext<AuthDefaultDbContext>(opt =>
                                                            opt.UseNpgsql(settings!.ConnectionString)
                                                               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
                                                    ServiceLifetime.Singleton);
        
        return services; 
    }
}