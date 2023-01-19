using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Message = Botticelli.Shared.ValueObjects.Message;

namespace Botticelli.Framework.Telegram.Handlers;

public class BotUpdateHandler : IUpdateHandler
{
    private readonly IList<IClientMessageProcessor> _clientProcessors = new List<IClientMessageProcessor>();

    private readonly ManualResetEventSlim _startEventSlim = new(false);

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient,
                                              Exception exception,
                                              CancellationToken cancellationToken) =>
            throw new NotImplementedException();

    public async Task HandleUpdateAsync(ITelegramBotClient botClient,
                                        Update update,
                                        CancellationToken cancellationToken)
    {
        try
        {
            var botMessage = update.Message;

            var botticelliMessage = new Message(Guid.Empty.ToString())
            {
                ChatId = botMessage.MessageId.ToString(),
                Subject = string.Empty,
                Body = botMessage.Text,
                Attachments = new List<BinaryAttachment>(5)
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
        while (true)
        {
            if (token is {CanBeCanceled: true, IsCancellationRequested: true}) break;

            var clientTasks = _clientProcessors.Select(p => ProcessForProcessor(p, request, token));

            Task.WhenAll(clientTasks);
        }
    }

    /// <summary>
    ///     Request processing for a particular processor
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private Task ProcessForProcessor(IMessageProcessor processor, Message request, CancellationToken token)
    {
        return Task.Run(() =>
                        {
                            _startEventSlim.Wait(token);
                            processor.ProcessAsync(request, token);
                        },
                        token);
    }

    public void AddClientEventProcessor(IClientMessageProcessor messageProcessor)
    {
        _clientProcessors.Add(messageProcessor);
    }
}