using Botticelli.Bot.Interfaces.Processors;

namespace Botticelli.Framework.Commands.Processors;

/// <summary>
/// A command chain processor means, that we've a command on on input and several step to process it
/// During a processing every processor changes values in 
/// </summary>
/// <typeparam name="TInputCommand"></typeparam>
public interface ICommandChainProcessor<TInputCommand> : ICommandChainProcessor
{
}

public interface ICommandChainProcessor : ICommandProcessor
{
    public ICommandProcessor Next { get; set; }
}