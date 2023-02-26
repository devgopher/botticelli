using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Telegram;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using TelegramBotSample.Commands;

namespace TelegramBotSample;

public class AiCommandProcessor : CommandProcessor<AiCommand>
{
    private readonly IBotticelliBusClient _bus;

    public AiCommandProcessor(IBot<TelegramBot> bot,
                              ILogger<AiCommandProcessor> logger,
                              ICommandValidator<AiCommand> validator,
                              IBotticelliBusClient bus)
            : base(bot, logger, validator) =>
            _bus = bus;

    protected override async Task InnerProcess(Message message, CancellationToken token)
    {
        var uid = string.Empty;

        var response = await _bus.GetResponse(new SendMessageRequest(uid)
                                              {
                                                  Message = new AiMessage(uid)
                                                  {
                                                      ChatId = message.ChatId,
                                                      Subject = string.Empty,
                                                      Body = message.Body,
                                                      Attachments = null,
                                                      From = null,
                                                      ForwardFrom = null
                                                  }
                                              },
                                              token);
    }
}