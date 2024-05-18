﻿using System.Globalization;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Layouts.Inlines;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.SendOptions;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramInlineLayoutsSample.Commands;

namespace TelegramInlineLayoutsSample.Handlers;

public class GetCalendarCommandProcessor : CommandProcessor<GetCalendarCommand>
{
    private readonly IBot _bot;
    private SendOptionsBuilder<InlineKeyboardMarkup> _options;

    public GetCalendarCommandProcessor(IBot bot,
                                       ICommandValidator<GetCalendarCommand> validator,
                                       MetricsProcessor metricsProcessor,
                                       ILayoutSupplier<InlineKeyboardMarkup> supplier,
                                       ILogger<GetCalendarCommandProcessor> logger) : base(logger, validator, metricsProcessor)
    {
        _bot = bot;

        InitLayouts(supplier);
    }

    private void InitLayouts(ILayoutSupplier<InlineKeyboardMarkup> supplier)
    {
        var markup = supplier.GetMarkup(CalendarFactory.Get(DateTime.Now, CultureInfo.InvariantCulture.Name));
        _options = SendOptionsBuilder<InlineKeyboardMarkup>.CreateBuilder(markup);
    }

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