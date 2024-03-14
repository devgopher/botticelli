using AiSample.Common.Commands;
using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

namespace AiSample.Common;

public class AiCommandProcessor : CommandProcessor<AiCommand>
{
    private readonly IEventBusClient _bus;

    public AiCommandProcessor(ILogger<AiCommandProcessor> logger,
        ICommandValidator<AiCommand> validator,
        MetricsProcessor metricsProcessor,
        IEventBusClient bus)
        : base(logger, validator, metricsProcessor)
    {
        _bus = bus;

        _bus.OnReceived += (sender, response) =>
        {
            if (response != null)
                _bot.SendMessageAsync(new SendMessageRequest(response.Uid)
                    {
                        Message = response.Message,
                        ExpectPartialResponse = response.IsPartial
                    },
                    SendOptionsBuilder<ReplyMarkupBase>.CreateBuilder(new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton[]
                        {
                            "/ai Thank you!",
                            "/ai Good bye!",
                            "/ai Tell me smth interesting"
                        }
                    })
                    {
                        ResizeKeyboard = true
                    }),
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
        _bus.Send(new SendMessageRequest(message.Uid)
        {
            Message = new AiMessage(message.Uid)
            {
                ChatIds = message.ChatIds,
                Subject = string.Empty,
                Body = message.Body
                    .Replace("/ai", string.Empty)
                    .Trim(),
                Attachments = null,
                From = message.From,
                ForwardedFrom = message.ForwardedFrom
            }
        });
}