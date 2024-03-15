using Botticelli.AI.Message;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.AI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAiValidation(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssemblyContaining<AiMessage>(ServiceLifetime.Singleton);
    }
}