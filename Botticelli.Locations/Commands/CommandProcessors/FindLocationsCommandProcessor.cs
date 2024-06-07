using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.BasicControls;
using Botticelli.Framework.Controls.Layouts;
using Botticelli.Framework.Controls.Layouts.Inlines;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.SendOptions;
using Botticelli.Locations.Integration;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Locations.Commands.CommandProcessors;

public class FindLocationsCommandProcessor<TReplyMarkup> : CommandProcessor<FindLocationsCommand> 
        where TReplyMarkup : class
{
    private readonly ILocationProvider _locationProvider;
    private readonly ILayoutSupplier<TReplyMarkup> _layoutSupplier;


    public FindLocationsCommandProcessor(ILogger<FindLocationsCommandProcessor<TReplyMarkup>> logger, 
                                         ICommandValidator<FindLocationsCommand> validator, 
                                         MetricsProcessor metricsProcessor,
                                         ILocationProvider locationProvider,
                                         ILayoutSupplier<TReplyMarkup> layoutSupplier) : base(logger, validator, metricsProcessor)
    {
        _locationProvider = locationProvider;
        _layoutSupplier = layoutSupplier;
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var query = string.Join(" ", values: message.Body?.Split(" ").Skip(1) ?? Array.Empty<string>());
        
        var results = await _locationProvider.Search(query, 10);

        var markup = new Table(2);

        foreach (var result in results)
        {
            var cdata =  await _locationProvider.GetMapLink(result);
            
            markup.AddItem(new Item
            {
                Control = new Button
                {
                    Content = result.DisplayName,
                    CallbackData = $"/Map {cdata}"
                },
                Params = new ItemParams()
            });
        }
        
        var responseMarkup = _layoutSupplier.GetMarkup(markup);
        var replyOptions = SendOptionsBuilder<TReplyMarkup>.CreateBuilder(responseMarkup);
        
        var request = new SendMessageRequest
        {
            Message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = "Addresses",
            }
        };

        await Bot.SendMessageAsync(request,  replyOptions, token);
    }
}