using System.Text.Json;
using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Bot.Utils;
using Botticelli.Client.Analytics;
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

    protected abstract string CommandText { get; }

    public async Task ProcessAsync(Message message, CancellationToken token)
    {
        Logger.LogDebug("{processorName}.ProcessAsync() : processing a message {messageUid}: {message}",
            nameof(FluentCommandProcessor<TCommand>),
            message.Uid,
            JsonSerializer.Serialize(message));

        
        if (!CheckCommand(message))
        {
            Logger.LogDebug("{processorName}.ProcessAsync() : processing a message {messageUid}: failed",
                nameof(FluentCommandProcessor<TCommand>),
                message.Uid);

            return;
        }

        if (await commandValidator.Validate(message))
        {
            Logger.LogDebug("{processorName}.ProcessAsync() : processing a message {messageUid}: command is valid",
                nameof(FluentCommandProcessor<TCommand>),
                message.Uid);
            SendMetric();
            
            if(!string.IsNullOrWhiteSpace(message.Body) || !string.IsNullOrWhiteSpace(message.CallbackData))
                await InnerProcess(message, token);
            
            if (message.Location != null) await InnerProcessLocation(message, token);
            if (message.Poll != null) await InnerProcessPoll(message, token);
            if (message.Contact != null) await InnerProcessContact(message, token);
        }
        else
        {
            var errMessageRequest = new SendMessageRequest
            {
                Message =
                {
                    Body = commandValidator.Help()
                }
            };

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
        return message.Body?.ToLowerInvariant().Trim() == CommandText.ToLowerInvariant().Trim() || message.Location != null || message.Poll != null || message.Contact != null;
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

        await Bot.SendMessageAsync(request, options, token);
    }

    protected abstract Task InnerProcess(Message message, CancellationToken token);
}