using System.Globalization;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Layouts.Commands.InlineCalendar;
using Botticelli.Framework.Controls.Layouts.Inlines;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Controls.Layouts.CommandProcessors.InlineCalendar;

/// <summary>
/// Calendar command processor
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TReplyMarkup"></typeparam>
public class ICCommandProcessor<TCommand, TReplyMarkup> : CommandProcessor<TCommand>
        where TCommand : BaseCalendarCommand where TReplyMarkup : class
{
    private readonly ILayoutSupplier<TReplyMarkup> _layoutSupplier;

    public ICCommandProcessor(ILogger<ICCommandProcessor<TCommand, TReplyMarkup>> logger,
                              ICommandValidator<TCommand> validator,
                              ILayoutSupplier<TReplyMarkup> layoutSupplier,
                              MetricsProcessor metricsProcessor) 
            : base(logger, validator, metricsProcessor)
    {
        _layoutSupplier = layoutSupplier;
    }

    protected override Task InnerProcessContact(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override Task InnerProcessPoll(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override Task InnerProcessLocation(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        Inlines.InlineCalendar calendar;

        if (!DateTime.TryParse(args, out var dt)) return;
        
        if (typeof(TCommand) == typeof(MonthBackward))
            calendar = Calendars.GetMonthsForward(dt, CultureInfo.InvariantCulture.Name, -1);
        else if (typeof(TCommand) == typeof(MonthForward))
            calendar = Calendars.GetMonthsForward(dt, CultureInfo.InvariantCulture.Name, 1);
        if (typeof(TCommand) == typeof(YearBackward))
            calendar = Calendars.GetMonthsForward(dt, CultureInfo.InvariantCulture.Name, -12);
        else if (typeof(TCommand) == typeof(YearForward))
            calendar = Calendars.GetMonthsForward(dt, CultureInfo.InvariantCulture.Name, 12);
        else
            calendar = Calendars.Get(DateTime.Now, CultureInfo.InvariantCulture.Name);
            
        var responseMarkup = _layoutSupplier.GetMarkup(calendar);
        var options = SendOptionsBuilder<TReplyMarkup>.CreateBuilder(responseMarkup);
        
        await Bot.SendMessageAsync(new SendMessageRequest
                                   {
                                       ExpectPartialResponse = false,
                                       Message = new Message
                                       {
                                           Uid = message.Uid,
                                           ChatIds = message.ChatIds
                                       }
                                   },
                                   options,
                                   token);
    }
}