using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Message = Botticelli.Shared.ValueObjects.Message;
using User = Botticelli.Shared.ValueObjects.User;

namespace Botticelli.Framework.Telegram.Handlers;

public class BotUpdateHandler : IUpdateHandler
{
    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient,
                                              Exception exception,
                                              CancellationToken cancellationToken)

    {
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient,
                                        Update update,
                                        CancellationToken cancellationToken)
    {
        try
        {
            var botMessage = update.Message;

            if (botMessage == null) return;

            var botticelliMessage = new Message(botMessage.MessageId.ToString())
            {
                ChatId = botMessage.Chat.Id.ToString(),
                Subject = string.Empty,
                Body = botMessage.Text,
                Attachments = new List<IAttachment>(5),
                From = new User
                {
                    Id = botMessage.From?.Id.ToString(),
                    Name = botMessage.From?.FirstName,
                    Surname = botMessage.From?.LastName,
                    Info = string.Empty,
                    IsBot = botMessage.From?.IsBot,
                    NickName = botMessage.From?.Username
                },
                ForwardFrom = new User
                {
                    Id = botMessage.ForwardFrom?.Id.ToString(),
                    Name = botMessage.ForwardFrom?.FirstName,
                    Surname = botMessage.ForwardFrom?.LastName,
                    Info = string.Empty,
                    IsBot = botMessage.ForwardFrom?.IsBot,
                    NickName = botMessage.ForwardFrom?.Username
                }
            };

            Process(botticelliMessage, cancellationToken);
        }
        catch (Exception ex)
        {
            // TODO:
        }
    }

    /// <summary>
    ///     Processes requests
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    protected void Process(Message request, CancellationToken token)
    {
        if (token is {CanBeCanceled: true, IsCancellationRequested: true}) return;

        var clientTasks = ClientProcessorFactory
                          .GetProcessors()
                          .Select(p => ProcessForProcessor(p, request, token));

        Task.WhenAll(clientTasks);
    }

    /// <summary>
    ///     Request processing for a particular processor
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private Task ProcessForProcessor(IMessageProcessor processor, Message request, CancellationToken token) =>
            Task.Run(() =>
                     {
                         //_startEventSlim.Wait(token);
                         processor.ProcessAsync(request, token);
                     },
                     token);
}