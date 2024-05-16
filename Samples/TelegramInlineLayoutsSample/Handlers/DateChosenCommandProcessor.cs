using System.Globalization;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Layouts.Commands.InlineCalendar;
using Botticelli.Framework.Controls.Layouts.Inlines;
using Botticelli.Framework.SendOptions;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;

namespace TelegramInlineLayoutsSample.Handlers;

public class DateChosenCommandProcessor : CommandProcessor<DateChosenCommand>
{
    private readonly IBot _bot;

    public DateChosenCommandProcessor(IBot bot,
                                      ICommandValidator<DateChosenCommand> validator,
                                      MetricsProcessor metricsProcessor,
                                      ILogger<GetCalendarCommandProcessor> logger) : base(logger, validator, metricsProcessor) =>
            _bot = bot;

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

        request.Message.Body = message.CallbackData?.Replace("/DateChosen ", string.Empty) ?? string.Empty;

        await _bot.SendMessageAsync(request, token);
    }
}