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
    private IBot _bot;
    protected readonly ILogger Logger;
    protected readonly ICommandValidator<TCommand> Validator;
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
            if (await Validator.Validate(message.ChatId, message.Body))
            {
                await InnerProcess(message, token);
            }
            else
            {
                request.Message.Body = Validator.Help();
                await _bot.SendMessageAsync(request, token);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error in {GetType().Name}: {ex.Message}");
        }
    }

    protected abstract Task InnerProcess(Message message, CancellationToken token);

    public void SetBot(IBot bot)
        => _bot = bot;

    public void SetServiceProvider(IServiceProvider sp)
        => _sp = sp;
}