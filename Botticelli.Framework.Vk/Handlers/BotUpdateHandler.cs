using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.MessageProcessors;
using Botticelli.Framework.Vk.Messages.API.Responses;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Vk.Messages.Handlers;

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
        _logger.LogDebug($"{nameof(HandleUpdateAsync)}() started...");

        var botMessages = update?
            .Where(x => x.Type == "message_new")
            .ToList();

        foreach (var botMessage in botMessages)
        {
            if (botMessage == default) continue;

            try
            {
                var val = botMessage.Object["message"];
                var peerId = botMessage.Object.ContainsKey("peer_id")
                    ? val.ToString()
                    : val.AsObject()["peer_id"].ToString();

                var text = botMessage.Object.ContainsKey("text") ? val.ToString() : val.AsObject()["text"].ToString();

                var fromId = botMessage.Object.ContainsKey("from_id")
                    ? val.ToString()
                    : val.AsObject()["from_id"].ToString();

                var botticelliMessage = new Message(botMessage.EventId)
                {
                    ChatIds = new List<string>
                    {
                        peerId,
                        fromId
                    },
                    Subject = string.Empty,
                    Body = text,
                    Attachments = new List<IAttachment>(5),
                    From = new User
                    {
                        Id = fromId
                    },
                    ForwardedFrom = null,
                    Location = null
                };

                await Process(botticelliMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(HandleUpdateAsync)}() error", ex);
            }
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

    public event IBotUpdateHandler.MsgReceivedEventHandler? MessageReceived;

    /// <summary>
    ///     Processes requests
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    protected async Task Process(Message request, CancellationToken token)
    {
        _logger.LogDebug($"{nameof(Process)}({request.Uid}) started...");

        if (token is { CanBeCanceled: true, IsCancellationRequested: true }) return;

        var clientTasks = _processorFactory
            .GetProcessors()
            .Where(p => p.GetType() != typeof(ChatMessageProcessor))
            .Select(p => p.ProcessAsync(request, token));


        Task.WaitAll(clientTasks.ToArray());

        _logger.LogDebug($"{nameof(Process)}({request.Uid}) finished...");
    }
}