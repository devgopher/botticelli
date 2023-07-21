using Botticelli.Bot.Interfaces.Bus.Handlers;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Bus.ZeroMQ.Agent;
using Botticelli.Bus.ZeroMQ.Client;
using Botticelli.Bus.ZeroMQ.Settings;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Bus.ZeroMQ.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Uses a rabbit bus
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection UseZeroMqBusClient<TBot>(this IServiceCollection services, IConfiguration config)
            where TBot : IBot =>
            services.AddSingleton<IBotticelliBusClient, ZeroMqClient<TBot>>()
                    .AddSingleton(GetZeroMqBusSettings(config));

    private static ZeroMqBusSettings GetZeroMqBusSettings(IConfiguration config)
    {
        var settings = new ZeroMqBusSettings();
        config.GetSection(nameof(ZeroMqBusSettings)).Bind(settings);

        return settings;
    }

    /// <summary>
    ///     Uses a rabbit bus
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection UseZeroMqBusAgent<TBot, THandler>(this IServiceCollection services, IConfiguration config)
            where TBot : IBot
            where THandler : IHandler<SendMessageRequest, SendMessageResponse> =>
            services.AddHostedService<ZeroMqAgent<TBot, THandler>>()
                    .AddSingleton(GetZeroMqBusSettings(config));
}