using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Bus.Rabbit.Agent;
using Botticelli.Bus.Rabbit.Client;
using Botticelli.Bus.Rabbit.Settings;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Botticelli.Bus.Rabbit.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Uses a rabbit bus
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection UseRabbitBusClient<TBot>(this IServiceCollection services, IConfiguration config)
            where TBot : IBot =>
            services.AddSingleton<IBotticelliBusClient, RabbitClient<TBot>>()
                    .AddSingleton(GetRabbitBusSettings(config))
                    .AddSingleton<IConnectionFactory>(sp => AddConnectionFactory(GetRabbitBusSettings(config)));

    private static ConnectionFactory AddConnectionFactory(RabbitBusSettings settings) =>
            new()
            {
                Uri = new Uri(settings.Uri),
                VirtualHost = settings.VHost
            };

    private static RabbitBusSettings GetRabbitBusSettings(IConfiguration config)
    {
        var settings = new RabbitBusSettings();
        config.GetSection(nameof(RabbitBusSettings)).Bind(settings);

        return settings;
    }

    /// <summary>
    ///     Uses a rabbit bus
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection UseRabbitBusAgent<TBot, THandler>(this IServiceCollection services, IConfiguration config)
            where TBot : IBot where THandler : IHandler<SendMessageRequest, SendMessageResponse> =>
            services.AddSingleton<IBotticelliBusAgent<THandler>, RabbitAgent<TBot, THandler>>()
                    .AddSingleton(GetRabbitBusSettings(config))
                    .AddSingleton<IConnectionFactory>(sp => AddConnectionFactory(GetRabbitBusSettings(config)));
}