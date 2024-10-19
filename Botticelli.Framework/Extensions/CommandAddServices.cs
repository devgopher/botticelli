using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Framework.Commands;
using Botticelli.Framework.Commands.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Extensions;


public class CommandAddServices<TCommand> where TCommand : ICommand
{
    private readonly IServiceCollection _services;

    public CommandAddServices(IServiceCollection services) => _services = services;

    public CommandAddServices<TCommand> AddProcessor<TCommandProcessor>()
            where TCommandProcessor : class, ICommandProcessor
    {
        _services.AddScoped<TCommandProcessor>();
        
        return this;
    }

    public CommandAddServices<TCommand> AddValidator<TCommandValidator>() 
            where TCommandValidator : class, ICommandValidator<TCommand>
    {
        // validator chain needs to be implemented!
        _services.AddScoped<TCommandValidator>()
                 .AddScoped<ICommandValidator<TCommand>, TCommandValidator>();

        return this;
    }
}