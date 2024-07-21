using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

/// <summary>
///     A command with waiting for a response
/// </summary>
/// <typeparam name="TInputCommand"></typeparam>
public abstract class WaitForClientResponseCommandChainProcessor<TInputCommand> : CommandProcessor<TInputCommand>,
                                                                                  ICommandChainProcessor<TInputCommand>
        where TInputCommand : class, ICommand
{
    protected WaitForClientResponseCommandChainProcessor(ILogger<CommandChainProcessor<TInputCommand>> logger,
                                                         ICommandValidator<TInputCommand> validator,
                                                         MetricsProcessor metricsProcessor) : base(logger, validator, metricsProcessor)
    {
    }

    private TimeSpan Timeout { get; } = TimeSpan.FromMinutes(10);

    public virtual void SetBot(IBot bot) => Bot = bot;

    public override async Task ProcessAsync(Message message, CancellationToken token)
    {
        Classify(ref message);

        if (message.Type != Message.MessageType.Messaging)
        {
            await base.ProcessAsync(message, token);
            return;
        }

        foreach (var chatId in message.ChatIds)
        {
            if (DateTime.UtcNow - message.LastModifiedAt > Timeout) continue;

            var responseMessage = message;
            responseMessage.ChatIds = [chatId];
            responseMessage.ProcessingArgs ??= new List<string>();

            if (Next != null)
                await Next.ProcessAsync(responseMessage, token);
            else
                _logger.LogInformation("No Next command for message {uid}", message.Uid);
        }
    }

    public ICommandChainProcessor Next { get; set; }
}