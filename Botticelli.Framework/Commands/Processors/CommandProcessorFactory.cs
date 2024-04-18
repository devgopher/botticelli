using Botticelli.Bot.Interfaces.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Commands.Processors;

public class CommandProcessorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<IFluentCommand> _staticInstances = new(10);
    private readonly Dictionary<Type, Type> _types = new();

    public CommandProcessorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void AddCommandType(Type commandType, Type procType)
        => _types[commandType] = procType;

    public ICommandProcessor? Get(string command)
    {
        if (string.IsNullOrEmpty(command)) throw new ArgumentNullException(nameof(command), "Can't be null or empty!");

        var canonized = command.Length == 1
            ? command.ToUpperInvariant()
            : $"{command[..1].ToUpperInvariant()}{command[1..].ToLowerInvariant()}";

        if (_types.Keys.All(t => t.Name.Replace("Command", string.Empty) != canonized))
            return _serviceProvider.GetRequiredService<CommandProcessor<Unknown>>();

        var type = _types.First(k => k.Key
                                         .Name
                                         .Replace("Command", string.Empty) == canonized
                                     && k.Key.IsAssignableFrom(typeof(ICommand)) &&
                                     !k.Key.IsAssignableFrom(typeof(IFluentCommand)))
            .Value;

        return _serviceProvider.GetRequiredService(type) as ICommandProcessor;
    }


    public IFluentCommandProcessor? GetFluent(string command)
    {
        if (string.IsNullOrEmpty(command)) throw new ArgumentNullException(nameof(command), "Can't be null or empty!");

        var canonized = command.ToLowerInvariant().Trim();

        _staticInstances[0].var type = _types.First(k => k.Key
                .Name
                .Replace("Command", string.Empty) == canonized && k.Key.IsAssignableFrom(typeof(IFluentCommand)))
            .Value;

        return _serviceProvider.GetRequiredService(type) as IFluentCommandProcessor;
    }
}