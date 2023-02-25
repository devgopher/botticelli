using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Telegram;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using TelegramBotSample.Commands;

namespace TelegramBotSample;

public class AiCommandProcessor : CommandProcessor<AiCommand>
{
    private readonly IBotticelliBusClient _bus;

    public AiCommandProcessor(IBot<TelegramBot> botClient,
                              ILogger<AiCommandProcessor> logger,
                              ICommandValidator<AiCommand> validator,
                              IBotticelliBusClient bus)
            : base(botClient, logger, validator) =>
            _bus = bus;

    protected override async Task InnerProcess(long chatId, CancellationToken token, params string[] args)
    {
        var uid = string.Empty;

        var response = await _bus.GetResponse(new SendMessageRequest(uid)
                                              {
                                                  Message = new AiMessage(uid)
                                                  {
                                                      ChatId = chatId.ToString(),
                                                      Subject = string.Empty,
                                                      Body = string.Join(' ', args),
                                                      Attachments = null,
                                                      From = null,
                                                      ForwardFrom = null
                                                  }
                                              },
                                              token);
    }
}