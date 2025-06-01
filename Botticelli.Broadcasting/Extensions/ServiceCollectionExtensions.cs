using System.Configuration;
using Botticelli.Broadcasting.Settings;
using Botticelli.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Broadcasting.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds broadcasting to a bot
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddBroadcasting<TBot>(this IServiceCollection services, IConfiguration config)
            where TBot : IBot
    {
        var settings = config.Get<BroadcastingSettings>();

        if (settings == null) throw new ConfigurationErrorsException("Broadcasting settings are missing!");

        // TODO: add broadcasting service (IHostedService which uses injected IServiceProvider and creates a new IBot??)

        return services;
    }
}