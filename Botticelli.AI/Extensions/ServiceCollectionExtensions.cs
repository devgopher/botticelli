using Botticelli.AI.AIProvider;
using Botticelli.AI.Exceptions;
using Botticelli.AI.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.AI.Extensions;

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
        var settings = new AiBotSettings();
        config.Bind(nameof(AiBotSettings), settings);

        var gptSettings = new AiGptSettings();
        config.Bind(nameof(AiGptSettings), gptSettings);

        var aiSettings = settings.Settings.FirstOrDefault(ai => ai.AiName == "gptj");

        if (aiSettings == default) throw new AiException("No section for gptj!");

        services.Configure<AiSettings>(s =>
        {
            s.Url = aiSettings.Url;
            s.AiName = aiSettings.AiName;
        });

        services.Configure<AiGptSettings>(s =>
        {
            s.GenerateTokensLimit = gptSettings.GenerateTokensLimit;
            s.Temperature = gptSettings.Temperature;
            s.TopK = gptSettings.TopK;
            s.TopP = gptSettings.TopP;
        });

        services.AddSingleton<IAiProvider, GptJProvider>();

        return services;
    }
}