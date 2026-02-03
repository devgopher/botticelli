using System.Text.Json;
using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Bot.Utils;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Utils;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.SendOptions;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

public abstract class FluentCommandProcessor<TCommand>(
    ILogger logger,
    MetricsProcessor metricsProcessor,
    ICommandValidator<TCommand> commandValidator)
    : ICommandProcessor
    where TCommand : class, IFluentCommand
{
    private IBot? Bot { get; set; }
    protected readonly ILogger Logger = logger;

    protected abstract Message.MessageType MessageType { get; }

    protected abstract string CommandText { get; }

    protected static void Classify(ref Message message)
    {
        var body = GetBody(message);

        if (CommandUtils.SimpleCommandRegex.IsMatch(body) || CommandUtils.ArgsCommandRegex.IsMatch(body))
            message.Type = Message.MessageType.Command;
        else if (!string.IsNullOrWhiteSpace(message.CallbackData))
            message.Type = Message.MessageType.Extended;
        else if (message.Poll != null)
            message.Type = Message.MessageType.Poll;
        else if (message.Contact != null)
            message.Type = Message.MessageType.Contact;
        else if (message.Location != null)
            message.Type = Message.MessageType.Location;
        else
            message.Type = Message.MessageType.Messaging;
    }

    private static string GetBody(Message message)
    {
        return !string.IsNullOrWhiteSpace(message.CallbackData) ? message.CallbackData
            : !string.IsNullOrWhiteSpace(message.Body) ? message.Body
            : string.Empty;
    }
    
    public async Task ProcessAsync(Message message, CancellationToken token)
    {
        Logger.LogDebug("{processorName}.ProcessAsync() : processing a message {messageUid}: {message}",
            nameof(FluentCommandProcessor<TCommand>),
            message.Uid,
            JsonSerializer.Serialize(message));

        Classify(ref message);
        
        if (!CheckCommand(message))
        {
            logger.LogDebug("{ProcessorName}.ProcessAsync() : processing a message {MessageUid}: failed",
                            nameof(FluentCommandProcessor<TCommand>),
                            message.Uid);

            return;
        }

        if (await commandValidator.Validate(message))
        {
            logger.LogDebug("{ProcessorName}.ProcessAsync() : processing a message {MessageUid}: command is valid",
                            nameof(FluentCommandProcessor<TCommand>),
                            message.Uid);
            SendMetric();

            await InnerProcess(message, token);
            
            if (message.Location != null) await InnerProcessLocation(message, token);
            if (message.Poll != null) await InnerProcessPoll(message, token);
            if (message.Contact != null) await InnerProcessContact(message, token);
        }
        else
        {
            Logger.LogDebug("{processorName}.ProcessAsync() : processing a message {messageUid}: command is NOT valid!",
                nameof(FluentCommandProcessor<TCommand>),
                message.Uid);
        }
    }

    public void SetBot(IBot bot)
    {
        Bot = bot;
    }

    public void SetServiceProvider(IServiceProvider sp)
    {
    }

    private bool CheckCommand(Message message)
    {
        if (message.Type != MessageType)
            return false;
            
        if (string.IsNullOrWhiteSpace(message.Body) && string.IsNullOrWhiteSpace(message.CallbackData) &&
            (message.Location != null || message.Poll != null || message.Contact != null))
            return true;

        if (message.Body == null && message.CallbackData == null && message.Location == null && message.Poll == null &&
            message.Contact == null)
            return false;

        if (message.Body != null)
            return message.Body!.ToLowerInvariant().Trim().StartsWith(CommandText.ToLowerInvariant().Trim());

        return message.CallbackData != null && message.CallbackData!.ToLowerInvariant().Trim()
            .StartsWith(CommandText.ToLowerInvariant().Trim());
    }


    private void SendMetric()
    {
        Logger.LogDebug("{processorName}.SendMetric() : sending a metric...", nameof(FluentCommandProcessor<TCommand>));
        metricsProcessor.Process($"{GetType().Name.Replace("Processor", string.Empty)}Command",
            BotDataUtils.GetBotId()!);
    }

    protected virtual Task InnerProcessContact(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected virtual Task InnerProcessPoll(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected virtual Task InnerProcessLocation(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected async Task SendMessage<TReplyMarkup>(SendMessageRequest request,
        SendOptionsBuilder<TReplyMarkup>? options,
        CancellationToken token)
        where TReplyMarkup : class
    {
        if (Bot == null) return;

        await Bot.SendMessageAsync(request, options, token).ConfigureAwait(false);
    }

    protected abstract Task InnerProcess(Message message, CancellationToken token);
}