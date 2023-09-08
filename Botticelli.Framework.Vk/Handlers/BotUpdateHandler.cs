using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.MessageProcessors;
using Botticelli.Framework.Vk.API.Responses;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Vk.Handlers;

public class BotUpdateHandler : IBotUpdateHandler
{
    private readonly ILogger<BotUpdateHandler> _logger;
    private readonly ClientProcessorFactory _processorFactory;

    public BotUpdateHandler(ILogger<BotUpdateHandler> logger, ClientProcessorFactory processorFactory)
    {
        _logger = logger;
        _processorFactory = processorFactory;
    }

    public async Task HandleUpdateAsync(List<UpdateEvent> update, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug($"{nameof(HandleUpdateAsync)}() started...");

            var botMessages = update?
                              .Where(x => x.Type == "message_new")
                              .ToList();

            foreach (var botMessage in botMessages)
            {
                if (botMessage == default) continue;

                var botticelliMessage = new Message(botMessage.EventId)
                {
                    ChatIds = new List<string>
                    {
                        botMessage.Object["peer_id"]
                                  .GetInt32()
                                  .ToString()
                    },
                    Subject = string.Empty,
                    Body = botMessage.Object["text"].GetString(),
                    Attachments = new List<IAttachment>(5),
                    From = new User
                    {
                        Id = botMessage.Object["from_id"].ToString()
                    },
                    ForwardedFrom = null,
                    Location = null
                };

                await Process(botticelliMessage, cancellationToken);
            }


            //if (botMessage == null) return;

            //var botticelliMessage = new Message(botMessage.MessageId.ToString())
            //{
            //    ChatIds = new() {botMessage.Chat.Id.ToString()},
            //    Subject = string.Empty,
            //    Body = botMessage.Text,
            //    Attachments = new List<IAttachment>(5),
            //    From = new User
            //    {
            //        Id = botMessage.From?.Id.ToString(),
            //        Name = botMessage.From?.FirstName,
            //        Surname = botMessage.From?.LastName,
            //        Info = string.Empty,
            //        IsBot = botMessage.From?.IsBot,
            //        NickName = botMessage.From?.Username
            //    },
            //    ForwardedFrom = new User
            //    {
            //        Id = botMessage.ForwardFrom?.Id.ToString(),
            //        Name = botMessage.ForwardFrom?.FirstName,
            //        Surname = botMessage.ForwardFrom?.LastName,
            //        Info = string.Empty,
            //        IsBot = botMessage.ForwardFrom?.IsBot,
            //        NickName = botMessage.ForwardFrom?.Username
            //    },
            //    Location = botMessage.Location != null ?
            //            new GeoLocation
            //            {
            //                Latitude = (decimal) botMessage.Location?.Latitude,
            //                Longitude = (decimal) botMessage.Location?.Longitude
            //            } :
            //            null
            //};

            //await Process(botticelliMessage, cancellationToken);

            _logger.LogDebug($"{nameof(HandleUpdateAsync)}() finished...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(HandleUpdateAsync)}() error", ex);
        }
    }

    public event IBotUpdateHandler.MsgReceivedEventHandler? MessageReceived;

    /// <summary>
    ///     Processes requests
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    protected async Task Process(Message request, CancellationToken token)
    {
        _logger.LogDebug($"{nameof(Process)}({request.Uid}) started...");

        if (token is {CanBeCanceled: true, IsCancellationRequested: true}) return;

        var clientTasks = _processorFactory
                          .GetProcessors()
                          .Where(p => p.GetType() != typeof(ChatMessageProcessor))
                          .Select(p => p.ProcessAsync(request, token));


        Task.WaitAll(clientTasks.ToArray());

        _logger.LogDebug($"{nameof(Process)}({request.Uid}) finished...");
    }
}