using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Framework.Commands;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
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

    public static IServiceCollection RegisterBotCommand<TCommand, 
                                                        TCommandProcessor,
                                                        TCommandValidator,
                                                        TBot>(this IServiceCollection services)
            where TCommand : class, ICommand
            where TCommandProcessor : class, ICommandProcessor
            where TCommandValidator : class, ICommandValidator<TCommand>
            where TBot : IBot
    {
        services.AddScoped<TCommand>()
                .AddScoped<TCommandProcessor>()
                .AddScoped<ICommandValidator<TCommand>, TCommandValidator>();

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<CommandProcessorFactory>();

        factory?.AddCommandType(typeof(TCommand), typeof(TCommandProcessor));

        ClientProcessorFactory.AddProcessor<TCommandProcessor, TBot>(provider);

        return services;
    }
}