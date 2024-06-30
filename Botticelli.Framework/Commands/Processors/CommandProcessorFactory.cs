using System.Reflection;
using Botticelli.Bot.Interfaces.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace Botticelli.Framework.Commands.Processors;

public class CommandProcessorFactory
{
    private readonly Dictionary<Type, Type> _fluentTypes = new();
    private readonly Dictionary<Type, Type> _oldFashionedTypes = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;

    public CommandProcessorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _scope = _serviceProvider.CreateScope();
    }

    public void AddCommandType(Type commandType, Type procType)
    {
        if (commandType.IsAssignableFrom(typeof(IFluentCommand)))
        {
            _fluentTypes[commandType] = procType;

            return;
        }

        _oldFashionedTypes[commandType] = procType;
    }

    public ICommandProcessor? Get(string command)
    {
        if (string.IsNullOrEmpty(command)) throw new ArgumentNullException(nameof(command), "Can't be null or empty!");

        var canonized = CanonizeOldFashioned(command);

        if (_oldFashionedTypes.Keys.All(t => t.Name.Replace("Command", string.Empty) != canonized))
            return _scope.ServiceProvider.GetRequiredService<CommandProcessor<Unknown>>();

        var type = _oldFashionedTypes.First(k => k.Key
                                                     .Name
                                                     .Replace("Command", string.Empty) == canonized)
            .Value;

        return _scope.ServiceProvider.GetRequiredService(type) as ICommandProcessor;
    }

    private static string CanonizeOldFashioned(string command) =>
        command.Length == 1
            ? command.ToUpperInvariant()
            : $"{command[..1].ToUpperInvariant()}{command[1..].ToLowerInvariant()}";


    public IFluentCommandProcessor? GetFluent(string command)
    {
        if (string.IsNullOrEmpty(command)) throw new ArgumentNullException(nameof(command), "Can't be null or empty!");

        var canonized = CanonizeFluent(command);
        var targetType = GetTypeByCommand(canonized);

        return _scope.ServiceProvider.GetRequiredService(targetType) as IFluentCommandProcessor;
    }

    private static string CanonizeFluent(string command) => command.ToLowerInvariant().Trim();

    private Type GetTypeByCommand(string canonized) => (from fluentType in _fluentTypes.Keys
        let cmdBody = fluentType.GetProperty(nameof(IFluentCommand.Command), BindingFlags.Static)?.GetValue(null)
        where CanonizeFluent((string)cmdBody) == canonized
        select _fluentTypes[fluentType]).FirstOrDefault();
}