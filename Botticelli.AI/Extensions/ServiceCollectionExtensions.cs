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


    /// <summary>
    ///     Adds a provider for ChatGpt-based solution
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    /// <exception cref="AiException"></exception>
    public static IServiceCollection AddChatGptProvider(this IServiceCollection services, IConfiguration config)
    {
        var chatGptSettings = new ChatGptSettings();
        config.Bind(nameof(ChatGptSettings), chatGptSettings);

        services.Configure<ChatGptSettings>(s =>
        {
            s.Model = chatGptSettings.Model;
            s.Temperature = chatGptSettings.Temperature;
            s.ApiKey = chatGptSettings.ApiKey;
            s.Url = chatGptSettings.Url;
            s.AiName = chatGptSettings.AiName;
        });

        services.AddSingleton<IAiProvider, ChatGptProvider>();

        return services;
    }
}