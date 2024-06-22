using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Exceptions;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

/// <summary>
/// A command with waiting for a response
/// </summary>
/// <typeparam name="TInputCommand"></typeparam>
public abstract class WaitForClientResponseCommandChainProcessor<TInputCommand> : CommandProcessor<TInputCommand>, ICommandChainProcessor<TInputCommand>
        where TInputCommand : class, ICommand
{
    private readonly Dictionary<string, Message> _receivedMessages = new(100);
    private TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);


    protected WaitForClientResponseCommandChainProcessor(ILogger<CommandChainProcessor<TInputCommand>> logger,
                                                         ICommandValidator<TInputCommand> validator,
                                                         MetricsProcessor metricsProcessor) : base(logger, validator, metricsProcessor)
    {
        var bot = (BaseBot) Bot;
        bot.MessageReceived += (sender, args) =>
        {
            foreach (var chatId in (args.Message?.ChatIds)
                     .Where(chatId => _receivedMessages[chatId].LastModifiedAt < args.Message?.LastModifiedAt))
            {
                _receivedMessages[chatId] = args.Message;
            }
        };
    }
    
    public override async Task ProcessAsync(Message message, CancellationToken token)
    {
        await base.ProcessAsync(message, token);
        
        var started = DateTime.Now;
        Message responseMessage = null;
        
        while (DateTime.Now - started > Timeout)
        {
            var chatId = message.ChatIds.First()!;

            if (_receivedMessages[chatId].LastModifiedAt <= message.LastModifiedAt)
            {
                Thread.Sleep(50);
                continue;
            }

            responseMessage = _receivedMessages[chatId];
            break;
        }

        if (responseMessage == null)
        {
            _logger.LogInformation("No response from a client for message {uid}", message.Uid);

            return;
        }
        
        if (Next != null)
            await Next.ProcessAsync(responseMessage, token);
        else
            _logger.LogInformation("No Next command for message {uid}", message.Uid);
    }
    
    public ICommandChainProcessor Next { get; set; }
}