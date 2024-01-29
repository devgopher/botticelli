using Botticelli.AI.AIProvider;
using Botticelli.AI.Exceptions;
using Botticelli.AI.GptJ.Provider;
using Botticelli.AI.GptJ.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.AI.GptJ.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a provider for GPT-J-based solution
    ///     https://devforth.io/blog/gpt-j-is-a-self-hosted-open-source-analog-of-gpt-3-how-to-run-in-docker/
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    /// <exception cref="AiException"></exception>
    public static IServiceCollection AddGptJProvider(this IServiceCollection services, IConfiguration config)
    {
        var aiGptSettings = new AiGptSettings();
        config.Bind(nameof(AiGptSettings), aiGptSettings);

        services.Configure<AiGptSettings>(s =>
        {
            s.GenerateTokensLimit = aiGptSettings.GenerateTokensLimit;
            s.Temperature = aiGptSettings.Temperature;
            s.TopK = aiGptSettings.TopK;
            s.TopP = aiGptSettings.TopP;
            s.AiName = aiGptSettings.AiName;
            s.ApiKey = aiGptSettings.ApiKey;
            s.Url = aiGptSettings.Url;
        });

        services.AddSingleton<IAiProvider, GptJProvider>();

        return services;
    }
}