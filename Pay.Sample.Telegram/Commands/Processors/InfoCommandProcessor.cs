using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using FluentValidation;

namespace TelegramPayBot.Commands.Processors;

public class InfoCommandProcessor<TReplyMarkup>(
        ILogger<InfoCommandProcessor<TReplyMarkup>> logger,
        ICommandValidator<InfoCommand> commandValidator,
        MetricsProcessor metricsProcessor,
        IValidator<Message> messageValidator)
        : CommandProcessor<InfoCommand>(logger,
                                        commandValidator,
                                        messageValidator,
                                        metricsProcessor)
        where TReplyMarkup : class
{
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
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = "This is a test payment bot.\nEnjoy!"
            }
        };

        await SendMessage(greetingMessageRequest, token);
    }
}