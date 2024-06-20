using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Locations.Commands.CommandProcessors;

public class MapCommandProcessor<TReplyMarkup> : CommandProcessor<MapCommand>
    where TReplyMarkup : class
{
    public MapCommandProcessor(ILogger<MapCommandProcessor<TReplyMarkup>> logger,
                               ICommandValidator<MapCommand> validator,
                               MetricsProcessor metricsProcessor) : base(logger, validator, metricsProcessor)
    {
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var request = new SendMessageRequest
        {
            Message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = args,
            }
        };

        await Bot.SendMessageAsync(request,  token);
    }
}