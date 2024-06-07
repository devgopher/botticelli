using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Locations.Integration;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Locations.Commands.CommandProcessors;

public class MapCommandProcessor<TReplyMarkup> : CommandProcessor<MapCommand>
    where TReplyMarkup : class
{
    private readonly ILocationProvider _locationProvider;
    private readonly ILayoutSupplier<TReplyMarkup> _layoutSupplier;


    public MapCommandProcessor(ILogger<FindLocationsCommandProcessor<TReplyMarkup>> logger,
        ICommandValidator<MapCommand> validator,
        MetricsProcessor metricsProcessor,
        ILocationProvider locationProvider,
        ILayoutSupplier<TReplyMarkup> layoutSupplier) : base(logger, validator, metricsProcessor)
    {
        _locationProvider = locationProvider;
        _layoutSupplier = layoutSupplier;
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