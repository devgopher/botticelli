using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using EasyCaching.Core;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

/// <summary>
///     A command with waiting for a response
/// </summary>
/// <typeparam name="TInputCommand"></typeparam>
public abstract class WaitForClientResponseCommandChainProcessor<TInputCommand> : CommandProcessor<TInputCommand>,
                                                                                  ICommandChainProcessor<TInputCommand>
        where TInputCommand : class, ICommand
{
    private readonly Dictionary<string, State> _chatStates = new(100);

    protected WaitForClientResponseCommandChainProcessor(ILogger<CommandChainProcessor<TInputCommand>> logger,
                                                         ICommandValidator<TInputCommand> validator,
                                                         MetricsProcessor metricsProcessor,
                                                         IEasyCachingProviderFactory factory) : base(logger, validator, metricsProcessor)
    {
    }

    private TimeSpan Timeout { get; } = TimeSpan.FromMinutes(10);

    public virtual void SetBot(IBot bot)
    {
        Bot = bot;
    }

    public override async Task ProcessAsync(Message message, CancellationToken token)
    {
        await base.ProcessAsync(message, token);

        if (message.Type != Message.MessageType.Messaging) return;

        foreach (var chatId in message.ChatIds)
        {
            if (!_chatStates.ContainsKey(chatId)) _chatStates[chatId] = State.WaitingForMessage;
            
            // checks state for each chat in a message
            if (_chatStates[chatId] != State.WaitingForMessage) continue;

            if (DateTime.UtcNow - message.LastModifiedAt > Timeout) continue;

            var responseMessage = message;
            responseMessage.ChatIds = [chatId];
            responseMessage.ProcessingArgs ??= new List<string>();
            responseMessage.ProcessingArgs?.Add(message.Body);

            ChangeState(message, chatId);

            if (Next != null)
                await Next.ProcessAsync(responseMessage, token);
            else
                _logger.LogInformation("No Next command for message {uid}", message.Uid);
        }
    }

    public ICommandChainProcessor Next { get; set; }

    private void ChangeState(Message message, string chatId)
    {
        _chatStates[chatId] = message.Type switch
        {
            Message.MessageType.Command   => State.WaitingForMessage,
            Message.MessageType.Messaging => State.WaitingForCommand,
            _                             => State.WaitingForCommand
        };
    }

    private enum State
    {
        WaitingForCommand,
        WaitingForMessage
    }
}