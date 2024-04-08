using Botticelli.AI.AIProvider;
using Botticelli.AI.DeepSeekGpt.Provider;
using Botticelli.AI.DeepSeekGpt.Settings;
using Botticelli.AI.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.AI.DeepSeekGpt.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a provider for DeepSeek-based solution
    ///     https://devforth.io/blog/gpt-j-is-a-self-hosted-open-source-analog-of-gpt-3-how-to-run-in-docker/
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    /// <exception cref="AiException"></exception>
    public static IServiceCollection AddDeepSeekProvider(this IServiceCollection services, IConfiguration config)
    {
        var deepSeekGptSettings = new DeepSeekGptSettings();
        config.Bind(nameof(DeepSeekGptSettings), deepSeekGptSettings);

        services.Configure<DeepSeekGptSettings>(s =>
        {
            s.Temperature = deepSeekGptSettings.Temperature;
            s.AiName = deepSeekGptSettings.AiName;
            s.ApiKey = deepSeekGptSettings.ApiKey;
            s.Url = deepSeekGptSettings.Url;
            s.Model = deepSeekGptSettings.Model;
            s.MaxTokens = deepSeekGptSettings.MaxTokens;
            s.Instruction = deepSeekGptSettings.Instruction;
        });

        services.AddScoped<IAiProvider, DeepSeekGptProvider>();

        return services;
    }
}