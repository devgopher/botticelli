using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Framework.Commands;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Extensions;

public class CommandAddServices<TCommand>(IServiceCollection services)
        where TCommand : class, ICommand
{
    public CommandAddServices<TCommand> AddProcessor<TCommandProcessor, TConfiguration>(IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TCommandProcessor : class, ICommandProcessor
            where TConfiguration : class
    {
        services.Configure<TConfiguration>(configuration.GetSection(typeof(TConfiguration).Name));
        AddProcessor<TCommandProcessor>(serviceLifetime);

        return this;
    }

    public CommandAddServices<TCommand> AddProcessor<TCommandProcessor>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TCommandProcessor : class, ICommandProcessor
    {
        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton<TCommandProcessor>();
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped<TCommandProcessor>();
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<TCommandProcessor>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
        }

        ProcessorFactoryBuilder.AddProcessor<TCommandProcessor>(services);

        return this;
    }

    public CommandAddServices<TCommand> AddValidator<TCommandValidator, TConfiguration>(IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TCommandValidator : class, ICommandValidator<TCommand>
            where TConfiguration : class
    {
        services.Configure<TConfiguration>(configuration.GetSection(typeof(TConfiguration).Name));

        // validator chain needs to be implemented!
        AddValidator<TCommandValidator>(serviceLifetime);

        return this;
    }

    public CommandAddServices<TCommand> AddValidator<TCommandValidator>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TCommandValidator : class, ICommandValidator<TCommand>
    {
        // validator chain needs to be implemented!
        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton<TCommandValidator>()
                    .AddSingleton<ICommandValidator<TCommand>, TCommandValidator>();
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped<TCommandValidator>()
                    .AddScoped<ICommandValidator<TCommand>, TCommandValidator>();
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<TCommandValidator>()
                    .AddTransient<ICommandValidator<TCommand>, TCommandValidator>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
        }

        return this;
    }
}