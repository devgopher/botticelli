using Botticelli.AI.AIProvider;
using Botticelli.AI.Exceptions;
using Botticelli.AI.YaGpt.Provider;
using Botticelli.AI.YaGpt.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.AI.YaGpt.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a provider for Yandex Gpt-based solution
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    /// <exception cref="AiException"></exception>
    public static IServiceCollection AddYaGptProvider(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<YaGptSettings>(config.GetSection(nameof(YaGptSettings)));
        services.AddSingleton<IAiProvider, YaGptProvider>();

        return services;
    }
}