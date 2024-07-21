using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using EasyCaching.Core;

namespace TelegramCommandChainSample.Commands.CommandProcessors;

public class SayHelloCommandProcessor : WaitForClientResponseCommandChainProcessor<SayHelloCommand>
{
    public SayHelloCommandProcessor(ILogger<CommandChainProcessor<SayHelloCommand>> logger,
        ICommandValidator<SayHelloCommand> validator,
        MetricsProcessor metricsProcessor,
        IEasyCachingProviderFactory factory) : base(logger, validator, metricsProcessor)
    {
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var responseMessage = new Message
        {
            ChatIdInnerIdLinks = message.ChatIdInnerIdLinks,
            ChatIds = message.ChatIds,
            Subject = string.Empty,
            Body = "What's your name?"
        };

        await Bot.SendMessageAsync(new SendMessageRequest
        {
            Message = responseMessage
        }, token);
    }
}