using Botticelli.Framework.Commands;
using Botticelli.Framework.Commands.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBotticelliFramework(this IServiceCollection services)
    {
        services.AddSingleton<CommandProcessorFactory>();

        return services;
    }

    public static IServiceCollection RegisterBotCommand<TCommand, TCommandProcessor>(this IServiceCollection services)
            where TCommand : class, ICommand
            where TCommandProcessor : class, ICommandProcessor
    {
        var factory = services.BuildServiceProvider().GetRequiredService<CommandProcessorFactory>();

        services.AddScoped<TCommand>()
                .AddScoped<TCommandProcessor>();

        factory?.AddCommandType(typeof(TCommand), typeof(TCommandProcessor));

        return services;
    }
}