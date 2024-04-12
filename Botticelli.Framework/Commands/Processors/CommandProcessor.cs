using System.Text.RegularExpressions;
using Botticelli.Analytics.Shared.Metrics;
using Botticelli.Bot.Interfaces.Processors;
using Botticelli.BotBase.Utils;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

public abstract partial class CommandProcessor<TCommand> : ICommandProcessor
    where TCommand : class, ICommand
{
    private readonly string _commandName;
    private readonly ILogger _logger;
    private readonly MetricsProcessor _metricsProcessor;
    private readonly ICommandValidator<TCommand> _validator;
    protected IBot Bot;

    protected CommandProcessor(ILogger logger,
        ICommandValidator<TCommand> validator,
        MetricsProcessor metricsProcessor)
    {
        _logger = logger;
        _validator = validator;
        _metricsProcessor = metricsProcessor;
        _commandName = GetCommandName(typeof(TCommand).Name);
    }

    public async Task ProcessAsync(Message message, CancellationToken token)
    {
        var request = SendMessageRequest.GetInstance();
        request.Message = new Message(Guid.NewGuid().ToString());

        try
        {
            if (string.IsNullOrWhiteSpace(message.Body) &&
                message.Attachments == default &&
                message.Location == default &&
                message.Contact == default &&
                message.Poll == default)
            {
                _logger.LogWarning("Message {msgId} is empty! Skipping...", message.Uid);

                return;
            }

            message.Body ??= string.Empty;

            if (SimpleCommandRegex().IsMatch(message.Body))
            {
                var match = SimpleCommandRegex().Matches(message.Body)
                    .FirstOrDefault();

                if (match == default) return;

                var commandName = GetCommandName(match.Groups[1].Value);

                if (commandName != _commandName) return;

                await ValidateAndProcess(message,
                    string.Empty,
                    request,
                    token);

                SendMetric(MetricNames.CommandReceived);
            }
            else if (ArgsCommandRegex().IsMatch(message.Body))
            {
                var match = ArgsCommandRegex().Matches(message.Body)
                    .FirstOrDefault();

                if (match == default) return;

                var argsString = match.Groups[2].Value;

                var commandName = GetCommandName(match.Groups[1].Value);

                if (commandName != _commandName) return;

                await ValidateAndProcess(message,
                    argsString,
                    request,
                    token);

                SendMetric(MetricNames.CommandReceived);
            }

            if (message.Location != default) await InnerProcessLocation(message, string.Empty, token);
            if (message.Poll != default) await InnerProcessPoll(message, string.Empty, token);
            if (message.Contact != default) await InnerProcessContact(message, string.Empty, token);
        }
        catch (Exception ex)
        {
            _metricsProcessor.Process(MetricNames.BotError, BotDataUtils.GetBotId());
            _logger.LogError(ex, $"Error in {GetType().Name}: {ex.Message}");
        }
    }

    public void SetBot(IBot bot)
        => Bot = bot;

    public void SetServiceProvider(IServiceProvider sp)
    {
    }

    private void SendMetric(string metricName) => _metricsProcessor.Process(metricName, BotDataUtils.GetBotId()!);

    private void SendMetric() => _metricsProcessor.Process(GetCommandName(
        $"{GetType().Name.Replace("Processor", string.Empty)}Command"), BotDataUtils.GetBotId()!);

    private string GetCommandName(string fullCommand)
        => fullCommand.ToLowerInvariant().Replace("command", "");

    private async Task ValidateAndProcess(Message message,
        string args,
        SendMessageRequest request,
        CancellationToken token)
    {
        if (await _validator.Validate(message.ChatIds, message.Body))
        {
            SendMetric();
            await InnerProcess(message, args, token);
        }
        else
        {
            request.Message.Body = _validator.Help();

            await Bot.SendMessageAsync(request, token);
        }
    }

    protected abstract Task InnerProcessContact(Message message, string args, CancellationToken token);
    protected abstract Task InnerProcessPoll(Message message, string args, CancellationToken token);
    protected abstract Task InnerProcessLocation(Message message, string args, CancellationToken token);
    protected abstract Task InnerProcess(Message message, string args, CancellationToken token);
   
    [GeneratedRegex("\\/([a-zA-Z0-9]*)$")]
    private static partial Regex SimpleCommandRegex();
    [GeneratedRegex("\\/([a-zA-Z0-9]*) (.*)")]
    private static partial Regex ArgsCommandRegex();
}