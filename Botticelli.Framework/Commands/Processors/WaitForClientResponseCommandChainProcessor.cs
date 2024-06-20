using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Commands.Processors;

public abstract class WaitForClientResponseCommandChainProcessor<TInputCommand> : CommandProcessor<TInputCommand>, ICommandChainProcessor<TInputCommand>
        where TInputCommand : class, ICommand
{
    private Dictionary<string, Message> _receivedMessages = new(100);
    
    protected WaitForClientResponseCommandChainProcessor(ILogger<CommandChainProcessor<TInputCommand>> logger,
                                                         ICommandValidator<TInputCommand> validator,
                                                         MetricsProcessor metricsProcessor) : base(logger, validator, metricsProcessor)
    {
        var bot = (BaseBot) Bot;
        bot.MessageReceived += (sender, args) =>
        {
            foreach (var chatId in args.Message?.ChatIds)
            {
                _receivedMessages[chatId] = args.Message;
                
                // Sort by LastModified field 
                args.Message.LastModifiedAt
                
            }
        };


    }
    
    public override async Task ProcessAsync(Message message, CancellationToken token)
    {
        await base.ProcessAsync(message, token);


        await Next.ProcessAsync(message, token);
    }

    
    
    public ICommandChainProcessor Next { get; set; }
}