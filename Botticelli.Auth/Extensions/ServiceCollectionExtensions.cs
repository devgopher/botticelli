using Botticelli.Auth.Dto.Credentials;
using Botticelli.Auth.Dto.User;
using Botticelli.Auth.Mapping;
using Botticelli.Auth.Rules;
using Botticelli.Auth.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Auth.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a basic authorization implementation
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBasicBotUserAuth(this IServiceCollection services)
    {
        MappingProfile.UseProfile();

        return services.AddSingleton<IIdentifier<BotAuthCredentials, BotUserInfo>, BasicBotAuthService>()
                       .AddSingleton<IManager<BotUserInfo>, DefaultUserManager>()
                       .AddSingleton<IManager<BotUserRoleInfo>, DefaultUserRoleManager>()
                       .AddSingleton<CredentialsRulesBuilder<BotAuthCredentials>>();
    }
}