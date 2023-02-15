using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Bot.Interfaces.Handlers;
using Botticelli.Bus.Rabbit.Agent;
using Botticelli.Bus.Rabbit.Client;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Responses;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Botticelli.Bus.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Uses a no-bus scheme
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection UseRabbitBusClient<TBot>(this IServiceCollection services)
    where TBot : IBot
    {
        services
        return services.AddScoped<IBotticelliBusClient, RabbitClient<TBot>>();
    }

    /// <summary>
    /// Uses a no-bus scheme
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection UseRabbitBusAgent<THandler, TBot>(this IServiceCollection services)
            where TBot : IBot where THandler : IHandler<SendMessageResponse> =>
            services.AddScoped<IBotticelliBusAgent<THandler>, RabbitAgent<TBot>>();
}