using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Framework.Commands;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.MessageProcessors;
using Botticelli.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBotticelliFramework(this IServiceCollection services)
    {
        services.AddSingleton<ChatMessageProcessor>()
                .AddSingleton<ClientProcessorFactory>()
                .AddSingleton<CommandProcessorFactory>();

        return services;
    }

    public static IServiceCollection AddBotCommand<TCommand,
                                                   TCommandProcessor,
                                                   TCommandValidator>(this IServiceCollection services)
            where TCommand : class, ICommand
            where TCommandProcessor : class, ICommandProcessor
            where TCommandValidator : class, ICommandValidator<TCommand>
        => services.AddScoped<TCommand>()
                   .AddScoped<TCommandProcessor>()
                   .AddScoped<ICommandValidator<TCommand>, TCommandValidator>();

    public static IServiceProvider RegisterBotCommand<TCommand, TCommandProcessor, TBot>(this IServiceProvider sp)
            where TCommand : class, ICommand
            where TCommandProcessor : class, ICommandProcessor
            where TBot : IBot
    {
        var commandProcessorFactory = sp.GetRequiredService<CommandProcessorFactory>();
        var clientProcessorFactory = sp.GetRequiredService<ClientProcessorFactory>();

        commandProcessorFactory.AddCommandType(typeof(TCommand), typeof(TCommandProcessor));
        clientProcessorFactory.AddProcessor<TCommandProcessor, TBot>(sp);
        clientProcessorFactory.AddChatMessageProcessor(sp.GetRequiredService<IBot>(), sp);

        return sp;
    }
}