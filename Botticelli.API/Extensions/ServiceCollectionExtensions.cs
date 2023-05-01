using Botticelli.BotBase.Settings;
using Botticelli.Framework.Extensions;
using Botticelli.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.BotBase.Extensions;

public static class ServiceCollectionExtensions
{
    [Obsolete]
    public static IServiceCollection UseBotticelli<TBot>(this IServiceCollection services,
                                                         IConfiguration config,
                                                         TBot bot)
            where TBot : IBot<TBot>
    {
        return services;
    }
}