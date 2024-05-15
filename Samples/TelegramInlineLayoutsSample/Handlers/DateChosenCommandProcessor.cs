using System.Globalization;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Layouts.Commands.InlineCalendar;
using Botticelli.Framework.Controls.Layouts.Inlines;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.SendOptions;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramInlineLayoutsSample.Handlers;

public class DateChosenCommandProcessor : CommandProcessor<DateChosenCommand>
{
    private readonly IBot _bot;
    private SendOptionsBuilder<InlineKeyboardMarkup> _options;

    public DateChosenCommandProcessor(IBot bot,
                                      ICommandValidator<DateChosenCommand> validator,
                                      MetricsProcessor metricsProcessor,
                                      ILayoutSupplier<InlineKeyboardMarkup> supplier,
                                      ILogger<GetCalendarCommandProcessor> logger) : base(logger, validator, metricsProcessor)
    {
        _bot = bot;

        InitLayouts(supplier);
    }

    private void InitLayouts(ILayoutSupplier<InlineKeyboardMarkup> supplier)
    {
        var markup = supplier.GetMarkup(Calendars.Get(DateTime.Now, CultureInfo.InvariantCulture.Name));
        _options = SendOptionsBuilder<InlineKeyboardMarkup>.CreateBuilder(markup);
    }

    
    public DateChosenCommandProcessor(ILogger logger, ICommandValidator<DateChosenCommand> validator, MetricsProcessor metricsProcessor) : base(logger, validator, metricsProcessor)
    {
    }

    protected override Task InnerProcessContact(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override Task InnerProcessPoll(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override Task InnerProcessLocation(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var request = new SendMessageRequest
        {
            ExpectPartialResponse = false,
            Message = message
        };

        await _bot.SendMessageAsync(request, _options, token);
    }
}