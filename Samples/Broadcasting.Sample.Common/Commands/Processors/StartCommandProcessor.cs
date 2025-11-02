using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Broadcasting.Sample.Common.Commands.Processors;

public class StartCommandProcessor<TReplyMarkup> : CommandProcessor<StartCommand> where TReplyMarkup : class
{
    public StartCommandProcessor(ILogger<StartCommandProcessor<TReplyMarkup>> logger,
        ICommandValidator<StartCommand> commandValidator,
        IValidator<Message> messageValidator)
        : base(logger,
            commandValidator,
            messageValidator)
    {
    }


    protected override Task InnerProcessContact(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerProcessPoll(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerProcessLocation(Message message, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override async Task InnerProcess(Message message, CancellationToken token)
    {
        var greetingMessageRequest = new SendMessageRequest
        {
            Message = new Message
            {
                ChatIds = message.ChatIds,
                Body = "Bot started. Please, wait for broadcast messages from admin..."
            }
        };

        await SendMessage(greetingMessageRequest, token);
    }
}