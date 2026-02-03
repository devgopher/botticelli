using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Bot.Utils;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Utils;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.SendOptions;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

public abstract class CommandProcessor<TCommand> : ICommandProcessor
        where TCommand : class, ICommand
{
    protected readonly string _command;
    private readonly ICommandValidator<TCommand> _commandValidator;
    private readonly IValidator<Message> _messageValidator;
    private readonly MetricsProcessor? _metricsProcessor;
    protected readonly ILogger Logger;
    protected IBot? _bot;

    protected CommandProcessor(ILogger logger,
                               ICommandValidator<TCommand> commandValidator,
                               IValidator<Message> messageValidator)
    {
        Logger = logger;
        _commandValidator = commandValidator;
        _messageValidator = messageValidator;
        _command = GetOldFashionedCommandName(typeof(TCommand).Name);
    }

    protected CommandProcessor(ILogger logger,
                               ICommandValidator<TCommand> commandValidator,
                               IValidator<Message> messageValidator,
                               MetricsProcessor? metricsProcessor)
    {
        Logger = logger;
        _commandValidator = commandValidator;
        _metricsProcessor = metricsProcessor;
        _messageValidator = messageValidator;
        _command = GetOldFashionedCommandName(typeof(TCommand).Name);
    }

    public virtual async Task ProcessAsync(Message message, CancellationToken token)
    {
        try
        {
            var messageValidationResult = await _messageValidator.ValidateAsync(message, token);

            if (!messageValidationResult.IsValid)
            {
                _metricsProcessor?.Process(MetricNames.BotError, BotDataUtils.GetBotId());
                Logger.LogError($"Error in {GetType().Name} invalid input message:" +
                                $" {messageValidationResult.Errors.Select(e => $"({e.PropertyName} : {e.ErrorCode} : {e.ErrorMessage})")}");

                return;
            }

            if (message.Poll != null)
            {
                await InnerProcessPoll(message, token);

                return;
            }

            if (message.From?.Id != null && message.From!.Id!.Equals(_bot?.BotUserId, StringComparison.InvariantCulture)) return;

            Classify(ref message);

            if (string.IsNullOrWhiteSpace(message.Body) &&
                message.Attachments == null &&
                message.Location == null &&
                message.Contact == null &&
                message.Poll == null &&
                message.CallbackData == null)
            {
                Logger.LogWarning("Message {MsgId} is empty! Skipping...", message.Uid);

                return;
            }

            // if we've any callback data, lets assume , that it is a command, if not - see in a message body
            var body = GetBody(message);

            if (CommandUtils.SimpleCommandRegex.IsMatch(body))
            {
                var match = CommandUtils.SimpleCommandRegex.Matches(body)
                                        .FirstOrDefault();

                if (match == null) return;

                var commandName = GetOldFashionedCommandName(match.Groups[1].Value);

                if (commandName != _command) return;

                await ValidateAndProcess(message, token);

                SendMetric(MetricNames.CommandReceived);
            }
            else if (CommandUtils.ArgsCommandRegex.IsMatch(body))
            {
                var match = CommandUtils.ArgsCommandRegex.Matches(body)
                                        .FirstOrDefault();

                if (match == null) return;

                var commandName = GetOldFashionedCommandName(match.Groups[1].Value);

                if (commandName != _command) return;

                await ValidateAndProcess(message, token);

                SendMetric(MetricNames.CommandReceived);
            }
            else
            {
                if (GetType().IsAssignableTo(typeof(CommandChainProcessor<TCommand>)))
                {
                    if (message.Type is Message.MessageType.Command or Message.MessageType.Extended)
                    {
                        var match = CommandUtils.ArgsCommandRegex.Matches(body)
                            .FirstOrDefault();

                        if (match == null) return;

                        var commandName = GetOldFashionedCommandName(match.Groups[1].Value);

                        if (commandName != _command) return;
                    }

                    await ValidateAndProcess(message, token);
                    
                    SendMetric(MetricNames.CommandReceived);
                }
            }

            if (message.Location != null) await InnerProcessLocation(message, token);
            if (message.Poll != null) await InnerProcessPoll(message, token);
            if (message.Contact != null) await InnerProcessContact(message, token);
        }
        catch (Exception ex)
        {
            _metricsProcessor?.Process(MetricNames.BotError, BotDataUtils.GetBotId());
            Logger.LogError(ex, "Error in {Name}: {ExMessage}", GetType().Name, ex.Message);

            await InnerProcessError(message, ex, token);
        }
    }


    public virtual void SetBot(IBot bot)
    {
        _bot = bot;
    }

    public void SetServiceProvider(IServiceProvider sp)
    {
    }

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

    private void SendMetric(string metricName)
    {
        _metricsProcessor?.Process(metricName, BotDataUtils.GetBotId()!);
    }

    private void SendMetric()
    {
        _metricsProcessor?.Process(GetOldFashionedCommandName($"{GetType().Name.Replace("Processor", string.Empty)}Command"),
                                   BotDataUtils.GetBotId()!);
    }

    private string GetOldFashionedCommandName(string fullCommand)
    {
        return fullCommand.ToLowerInvariant().Replace("command", "");
    }

    private async Task ValidateAndProcess(Message message,
                                          CancellationToken token)
    {
        if (_bot == null) return;

        if (message.Type == Message.MessageType.Messaging)
        {
            SendMetric();

            await InnerProcess(message, token);

            return;
        }

        if (await _commandValidator.Validate(message))
        {
            SendMetric();
            await InnerProcess(message, token);
        }
        else
        {
            var errMessageRequest = new SendMessageRequest
            {
                Message =
                {
                    Body = _commandValidator.Help()
                }
            };

            await SendMessage(errMessageRequest, token);
        }
    }

    protected async Task DeleteMessage(DeleteMessageRequest request, CancellationToken token)
    {
        if (_bot == null) return;

        await _bot.DeleteMessageAsync(request, token);
    }

    protected async Task DeleteMessage(Message message, CancellationToken token)
    {
        if (_bot == null) return;

        foreach (var request in message.ChatIds.Select(chatId => new DeleteMessageRequest(message.Uid, chatId))) await _bot.DeleteMessageAsync(request, token);
    }

    protected async Task SendMessage(Message message, CancellationToken token)
    {
        if (_bot == null) return;

        var request = new SendMessageRequest
        {
            Message = message
        };

        await SendMessage(request, token);
    }

    protected async Task SendMessage(SendMessageRequest request, CancellationToken token)
    {
        if (_bot == null) return;

        await _bot.SendMessageAsync(request, token);
    }

    protected async Task SendMessage<TReplyMarkup>(SendMessageRequest request,
                                                   SendOptionsBuilder<TReplyMarkup>? options,
                                                   CancellationToken token)
            where TReplyMarkup : class
    {
        if (_bot == null) return;

        await _bot.SendMessageAsync(request, options, token);
    }

    protected async Task UpdateMessage<TSendOptions>(Message message,
                                                     ISendOptionsBuilder<TSendOptions>? options,
                                                     CancellationToken token)
            where TSendOptions : class
    {
        if (_bot == null) return;

        await _bot.UpdateMessageAsync(new SendMessageRequest
                                      {
                                          ExpectPartialResponse = false,
                                          Message = new Message
                                          {
                                              Body = message.CallbackData,
                                              Uid = message.Uid,
                                              ChatIds = message.ChatIds,
                                              ChatIdInnerIdLinks = message.ChatIdInnerIdLinks
                                          }
                                      },
                                      options,
                                      token);
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

    protected abstract Task InnerProcess(Message message, CancellationToken token);

    protected virtual Task InnerProcessError(Message message, Exception? ex, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}