using System.Text.RegularExpressions;
using Botticelli.Bot.Interfaces.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

public abstract class CommandProcessor<TCommand> : ICommandProcessor
        where TCommand : class, ICommand
{
    private const string SimpleCommandPattern = @"\/([a-zA-Z0-9]*)$";
    private const string ArgsCommandPattern = @"\/([a-zA-Z0-9]*) (.*)";
    protected readonly ILogger Logger;
    protected readonly ICommandValidator<TCommand> Validator;
    protected IList<IBot> _bots = new List<IBot>(10);
    private readonly string _commandName;
    protected IServiceProvider _sp;

    protected CommandProcessor(ILogger logger,
                               ICommandValidator<TCommand> validator)
    {
        Logger = logger;
        Validator = validator;
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
                Logger.LogWarning("Message {msgId} is empty! Skipping...", message.Uid);

                return;
            }

            message.Body ??= string.Empty;

            if (Regex.IsMatch(message.Body, SimpleCommandPattern))
            {
                var match = Regex.Matches(message.Body, SimpleCommandPattern)
                                 .FirstOrDefault();

                var commandName = GetCommandName(match.Groups[1].Value);

                if (commandName != _commandName) return;

                if (match == default) return;

                await ValidateAndProcess(message,
                                         string.Empty,
                                         token,
                                         request);
            }
            else if (Regex.IsMatch(message.Body, ArgsCommandPattern))
            {
                var match = Regex.Matches(message.Body, ArgsCommandPattern)
                                 .FirstOrDefault();
                var argsString = match.Groups[2].Value;

                var commandName = GetCommandName(match.Groups[1].Value);

                if (commandName != _commandName) return;

                await ValidateAndProcess(message,
                                         argsString,
                                         token,
                                         request);
            }

            if (message.Location != default) await InnerProcessLocation(message, string.Empty, token);
            if (message.Poll != default) await InnerProcessPoll(message, string.Empty, token);
            if (message.Contact != default) await InnerProcessContact(message, string.Empty, token);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error in {GetType().Name}: {ex.Message}");
        }
    }

    public void AddBot(IBot bot)
        => _bots.Add(bot);

    public void SetServiceProvider(IServiceProvider sp)
        => _sp = sp;

    public string GetCommandName(string fullCommand)
        => fullCommand.ToLowerInvariant().Replace("command", "");

    private async Task ValidateAndProcess(Message message,
                                          string args,
                                          CancellationToken token,
                                          SendMessageRequest request)
    {
        if (await Validator.Validate(message.ChatIds, message.Body))
        {
            await InnerProcess(message, args, token);
        }
        else
        {
            request.Message.Body = Validator.Help();

            foreach (var bot in _bots) await bot.SendMessageAsync(request, token);
        }
    }

    protected abstract Task InnerProcessContact(Message message, string args, CancellationToken token);
    protected abstract Task InnerProcessPoll(Message message, string args, CancellationToken token);
    protected abstract Task InnerProcessLocation(Message message, string args, CancellationToken token);

    protected abstract Task InnerProcess(Message message, string args, CancellationToken token);
}