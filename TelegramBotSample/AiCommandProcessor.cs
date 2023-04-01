using Botticelli.AI.Message;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using TelegramBotSample.Commands;

namespace TelegramBotSample;

public class AiCommandProcessor : CommandProcessor<AiCommand>
{
    private readonly IBotticelliBusClient _bus;

    public AiCommandProcessor(ILogger<AiCommandProcessor> logger,
                              ICommandValidator<AiCommand> validator,
                              IBotticelliBusClient bus)
            : base(logger, validator) =>
            _bus = bus;

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var response = await _bus.GetResponse(new SendMessageRequest(message.Uid)
                                              {
                                                  Message = new AiMessage(message.Uid)
                                                  {
                                                      ChatId = message.ChatId,
                                                      Subject = string.Empty,
                                                      Body = args,
                                                      Attachments = null,
                                                      From = message.From,
                                                      ForwardFrom = message.ForwardFrom
                                                  }
                                              },
                                              token);

        if (response != null)
        {
            foreach (var bot in _bots)
            {
                await bot.SendMessageAsync(new SendMessageRequest(response.Uid)
                                             {
                                                 Message = response.Message
                                             },
                                             token);
            }
        }
    }
}