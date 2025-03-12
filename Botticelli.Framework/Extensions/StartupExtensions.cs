using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Framework.Commands;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.HostedService;
using Botticelli.Interfaces;
using Botticelli.Shared.Extensions;
using EasyCaching.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection AddBotticelliFramework(this IServiceCollection services)
    {
        return services.AddSharedValidation()
                       .AddHostedService<BotHostedService>()
                       .AddEasyCaching(options =>
                       {
                           options.UseInMemory(config =>
                                               {
                                                   config.DBConfig = new InMemoryCachingOptions
                                                   {
                                                       ExpirationScanFrequency = 10,
                                                       // total count of cache items, default value is 10000
                                                       SizeLimit = 5000
                                                   };
                                                   // the max random second will be added to cache's expiration, default value is 120
                                                   config.MaxRdSecond = 10;
                                                   // whether enable logging, default is false
                                                   config.EnableLogging = false;
                                                   // mutex key's alive time(ms), default is 5000
                                                   config.LockMs = 500;
                                                   // when mutex key alive, it will sleep some time, default is 300
                                                   config.SleepMs = 30;
                                               },
                                               "botticelli_wait_for_response");
                       });
    }

    public static CommandAddServices<TCommand> AddBotCommand<TCommand>(this IServiceCollection services)
            where TCommand : class, ICommand
    {
        services.AddScoped<TCommand>()
                .AddSingleton<CommandAddServices<TCommand>>(_ => new CommandAddServices<TCommand>(services));

        return services.BuildServiceProvider().GetRequiredService<CommandAddServices<TCommand>>();
    }

    public static IServiceCollection AddBotCommand<TCommand,
                                                   TCommandProcessor,
                                                   TCommandValidator>(this IServiceCollection services)
            where TCommand : class, ICommand
            where TCommandProcessor : class, ICommandProcessor
            where TCommandValidator : class, ICommandValidator<TCommand>
    {
        return services.AddSingleton<TCommand>()
                       .AddSingleton<TCommandProcessor>()
                       .AddSingleton<ICommandValidator<TCommand>, TCommandValidator>();
    }

    public static CommandChainProcessorBuilder<TCommand> AddBotChainProcessedCommand<TCommand,
                                                                                     TCommandValidator>(this IServiceCollection services)
            where TCommand : class, ICommand where TCommandValidator : class, ICommandValidator<TCommand>
    {
        var builder = new CommandChainProcessorBuilder<TCommand>(services);

        services.AddSingleton<TCommand>()
                .AddSingleton<ICommandValidator<TCommand>, TCommandValidator>()
                .AddSingleton(_ => builder);

        return builder;
    }

    public static IServiceProvider RegisterBotChainedCommand<TCommand, TBot>(this IServiceProvider sp)
            where TCommand : class, ICommand
            where TBot : IBot<TBot>
    {
        var commandChainProcessorBuilder = sp.GetRequiredService<CommandChainProcessorBuilder<TCommand>>();
        commandChainProcessorBuilder.Build();

        return sp;
    }
}