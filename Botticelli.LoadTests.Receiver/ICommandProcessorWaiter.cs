using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Framework.Commands;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;

namespace Botticelli.LoadTests.Receiver;

/// <summary>
/// This interface is responsible for putting a command to a command processor and
/// waiting for an answer
/// </summary>
/// <typeparam name="TCommandProcessor">Command processor type</typeparam>
/// <typeparam name="TCommand">Command</typeparam>
public interface ICommandProcessorWaiter<TCommandProcessor, TCommand> : ICommandProcessor
where TCommandProcessor : CommandProcessor<TCommand> where TCommand : class, ICommand
{
    public Task<Message> PutCommand(ICommand command, CancellationToken token);

    public Task WaitForResponse(Message commandMessage);
}