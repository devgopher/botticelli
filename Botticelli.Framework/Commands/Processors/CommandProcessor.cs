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
    protected IBot _bot;
    protected IServiceProvider _sp;

    protected CommandProcessor(IBot bot,
                               ILogger logger,
                               ICommandValidator<TCommand> validator)
    {
        SetBot(bot);
        Logger = logger;
        Validator = validator;
    }

    public async Task ProcessAsync(Message message, CancellationToken token)
    {
        var request = SendMessageRequest.GetInstance();
        request.Message = new Message(Guid.NewGuid().ToString());

        try
        {
            if (Regex.IsMatch(message.Body, SimpleCommandPattern))
            {
                var match = Regex.Matches(message.Body, SimpleCommandPattern)
                                 .FirstOrDefault();

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

                await ValidateAndProcess(message,
                                         argsString,
                                         token,
                                         request);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error in {GetType().Name}: {ex.Message}");
        }
    }

    public void SetBot(IBot bot)
        => _bot = bot;

    public void SetServiceProvider(IServiceProvider sp)
        => _sp = sp;

    private async Task ValidateAndProcess(Message message,
                                          string args,
                                          CancellationToken token,
                                          SendMessageRequest request)
    {
        if (await Validator.Validate(message.ChatId, message.Body))
        {
            await InnerProcess(message, args, token);
        }
        else
        {
            request.Message.Body = Validator.Help();
            await _bot.SendMessageAsync(request, token);
        }
    }

    protected abstract Task InnerProcess(Message message, string args, CancellationToken token);
}