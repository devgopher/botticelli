﻿using AiSample.Common.Commands;
using AiSample.Common.Layouts;
using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.Layouts;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

namespace AiSample.Common;

public class AiCommandProcessor<TReplyMarkup> : CommandProcessor<AiCommand> where TReplyMarkup : class
{
    private readonly IEventBusClient _bus;

    public AiCommandProcessor(ILogger<AiCommandProcessor<TReplyMarkup>> logger,
        ICommandValidator<AiCommand> validator,
        MetricsProcessor metricsProcessor,
        IEventBusClient bus, 
        ILayoutSupplier<TReplyMarkup> layoutSupplier)
        : base(logger, validator, metricsProcessor)
    {
        _bus = bus;
        var responseLayout = new AiLayout();
        var responseMarkup = layoutSupplier.GetMarkup(responseLayout);
        
        var options = SendOptionsBuilder<TReplyMarkup>.CreateBuilder(responseMarkup);
        
        _bus.OnReceived += async (sender, response) =>
        {
            if (response != null)
                await Bot.SendMessageAsync(new SendMessageRequest(response.Uid)
                    {
                        Message = response.Message,
                        ExpectPartialResponse = response.IsPartial,
                        SequenceNumber = response.SequenceNumber,
                        IsFinal = response.IsFinal
                    },
                    options,
                    CancellationToken.None);
        };
    }

    protected override async Task InnerProcessContact(Message message, string argsString, CancellationToken token)
    {
    }

    protected override async Task InnerProcessPoll(Message message, string argsString, CancellationToken token)
    {
    }

    protected override async Task InnerProcessLocation(Message message, string argsString, CancellationToken token)
    {
        message.Body =
            $"{$"Coordinates {message.Location.Latitude:##.#####}".Replace(",", ".")},{$"{message.Location.Longitude:##.#####}".Replace(",", ".")}";
        await InnerProcess(message, argsString, token);
    }


    protected override async Task InnerProcess(Message message, string args, CancellationToken token) =>
        await _bus.Send(new SendMessageRequest(message.Uid)
        {
            Message = new AiMessage(message.Uid)
            {
                ChatIds = message.ChatIds,
                Subject = string.Empty,
                Body = message.Body?.Replace("/ai", string.Empty)
                    .Trim(),
                Attachments = null,
                From = message.From,
                ForwardedFrom = message.ForwardedFrom
            }
        }, token);
}