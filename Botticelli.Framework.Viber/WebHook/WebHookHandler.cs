using System.Net;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Viber.Messages;
using Botticelli.Framework.Viber.Messages.Callbacks;
using Botticelli.Framework.Viber.Options;
using Botticelli.Serialization;
using Botticelli.Shared.Constants;
using Botticelli.Shared.Utils;
using Botticelli.Shared.ValueObjects;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Viber.WebHook;

public class WebHookHandler
{
    private readonly HttpListener _listener;
    private readonly ILogger<WebHookHandler> _logger;
    private readonly ClientProcessorFactory _processorFactory;
    private readonly ISerializerFactory _serializerFactory;
    private readonly INotificator<ViberBot> _notificator;
    private readonly ViberBotSettings _settings;

    public WebHookHandler(ISerializerFactory serializerFactory,
                          ILogger<WebHookHandler> logger,
                          ViberBotSettings settings,
                          ClientProcessorFactory processorFactory,
                          INotificator<ViberBot> notificator)
    {
        _serializerFactory = serializerFactory;
        _logger = logger;
        _settings = settings;
        _processorFactory = processorFactory;
        _notificator = notificator;
        _listener = new HttpListener();
        _listener.Prefixes.Add(_settings.WebHookUrl);
    }

    public async Task Start(CancellationToken token)
    {
        _listener.Start();

        var listenerTask = Task.Run(async () =>
                                    {
                                        while (!token.IsCancellationRequested)
                                            try
                                            {
                                                var context = await _listener.GetContextAsync();
                                                using var response = context.Response;

                                                response.StatusCode = (int) HttpStatusCode.OK;
                                                response.StatusDescription = "OK";

                                                if (context.Request == default)
                                                {
                                                    _logger.LogDebug("Empty request!");

                                                    continue;
                                                }

                                                if (!context.Request
                                                            .ContentType
                                                            .ToLowerInvariant()
                                                            .Contains("json"))
                                                {
                                                    _logger.LogDebug($"Wrong content type: {context.Request.ContentType}");

                                                    continue;
                                                }

                                                var json = context.Request.InputStream.FromStream();

                                                var basicCallback = _serializerFactory.GetSerializer<BasicCallback>()
                                                                                      .Deserialize(json);


                                                var webhookMessage = basicCallback as WebHookMessage;

                                                if (webhookMessage == default)
                                                {
                                                    _logger.LogWarning($"This is not a webhook message: {basicCallback.MessageToken}");

                                                    continue;
                                                }

                                                var processors = _processorFactory.GetProcessors();


                                                switch (webhookMessage.Event)
                                                {
                                                    case EventTypes.Message:
                                                        var concreteTypeMessage = webhookMessage.Adapt<WebHookReceivedMessage>();

                                                        var botticelliMessage = MakeBotticelliMessage(concreteTypeMessage);

                                                        foreach (var processor in processors) await processor.ProcessAsync(botticelliMessage, token).ConfigureAwait(false);


                                                        _notificator.NotifyReceived(this, botticelliMessage);

                                                        break;
                                                    case EventTypes.ConversationStarted:
                                                    case EventTypes.Delivered:
                                                    case EventTypes.Failed:
                                                    case EventTypes.Seen:
                                                    case EventTypes.Subscribed:
                                                    case EventTypes.Unsubscribed:
                                                        _notificator.NotifyMessengerSpecific(this, webhookMessage.Event, 
                                                                                             new List<string>()
                                                                                             {
                                                                                                 $"{nameof(webhookMessage.MessageToken)}={webhookMessage.MessageToken}" 
                                                                                             });
                                                        break;
                                                    default: throw new NotImplementedException("Not implemented!");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                _logger.LogError(ex.Message, ex);
                                            }

                                        _listener.Stop();
                                    },
                                    token);
    }

    private static Message MakeBotticelliMessage(WebHookReceivedMessage concreteTypeMessage) =>
            new(concreteTypeMessage.MessageToken.ToString())
            {
                ChatId = concreteTypeMessage.ViberMessage?.TrackingData,
                Subject = string.Empty,
                Body = concreteTypeMessage.ViberMessage?.Text,
                Attachments = concreteTypeMessage.ViberMessage?.Media != null ?
                        new List<IAttachment>
                        {
                            new BinaryAttachment(Guid.NewGuid().ToString(),
                                                 string.Empty,
                                                 MediaType.Unknown,
                                                 concreteTypeMessage.ViberMessage?.Media,
                                                 Array.Empty<byte>())
                        } :
                        null,
                From = new User
                {
                    Id = concreteTypeMessage.Sender.Name,
                    Info = string.Empty,
                    IsBot = null,
                    NickName = concreteTypeMessage.Sender.Name
                },
                ForwardFrom = null
            };
}