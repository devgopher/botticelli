using Botticelli.Framework.Commands;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBotticelliFramework(this IServiceCollection services)
    {
        services.AddSingleton<CommandProcessorFactory>();

        return services;
    }

    public static IServiceCollection RegisterBotCommand<TCommand, TCommandProcessor, TBot>(this IServiceCollection services)
            where TCommand : class, ICommand
            where TCommandProcessor : class, ICommandProcessor, IClientMessageProcessor, new()
            where TBot : IBot
    {
        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<CommandProcessorFactory>();

        services.AddScoped<TCommand>()
                .AddScoped<TCommandProcessor>();

        factory?.AddCommandType(typeof(TCommand), typeof(TCommandProcessor));

        ClientProcessorFactory.AddProcessor<TCommandProcessor, TBot>(provider);

        return services;
    }
}