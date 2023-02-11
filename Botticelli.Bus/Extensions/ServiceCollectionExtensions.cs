using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Bus.Agent;
using Botticelli.Bus.Client;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Bus.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Uses a no-bus scheme
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection UsePassBusClient<TBot>(this IServiceCollection services)
    where TBot : IBot =>
            services.AddScoped<IBotticelliBusClient, PassClient<TBot>>();

    /// <summary>
    /// Uses a no-bus scheme
    /// </summary>
    /// <typeparam name="TBot"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection UsePassBusAgent<TBot>(this IServiceCollection services)
            where TBot : IBot =>
            services.AddScoped<IBotticelliBusAgent, PassAgent<TBot>>();
}