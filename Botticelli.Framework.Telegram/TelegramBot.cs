using BotDataSecureStorage;
using Botticelli.BotBase.Utils;
using Botticelli.Framework.Events;
using Botticelli.Framework.Exceptions;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Botticelli.Shared.Utils;
using Botticelli.Shared.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Poll = Botticelli.Shared.ValueObjects.Poll;

namespace Botticelli.Framework.Telegram;

public class TelegramBot : BaseBot<TelegramBot>
{
    private readonly IBotUpdateHandler _handler;
    private readonly SecureStorage _secureStorage;
    private ITelegramBotClient _client;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="client"></param>
    /// <param name="handler"></param>
    /// <param name="logger"></param>
    /// <param name="mediator"></param>
    /// <param name="secureStorage"></param>
    public TelegramBot(ITelegramBotClient client,
                       IBotUpdateHandler handler,
                       ILogger<TelegramBot> logger,
                       IMediator mediator,
                       SecureStorage secureStorage) : base(logger, mediator)
    {
        IsStarted = false;
        _client = client;
        _handler = handler;
        _secureStorage = secureStorage;

        // Migration to a bot context instead of simple bot key
        _secureStorage.MigrateToBotContext(BotDataUtils.GetBotId());
    }

    public override BotType Type => BotType.Telegram;
    public override event MsgSentEventHandler MessageSent;
    public override event MsgReceivedEventHandler MessageReceived;
    public override event MsgRemovedEventHandler MessageRemoved;
    public override event MessengerSpecificEventHandler MessengerSpecificEvent;

    /// <summary>
    ///     Deletes a message
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="BotException"></exception>
    public override async Task<RemoveMessageResponse> DeleteMessageAsync(RemoveMessageRequest request, CancellationToken token)
    {
        if (!IsStarted)
        {
            Logger.LogInformation("Bot wasn't started!");

            return new RemoveMessageResponse(request.Uid, "Bot wasn't started!")
            {
                MessageRemovedStatus = MessageRemovedStatus.NotStarted
            };
        }

        RemoveMessageResponse response = new(request.Uid, string.Empty);

        try
        {
            if (string.IsNullOrWhiteSpace(request.Uid)) throw new BotException("request/message is null!");

            await _client.DeleteMessageAsync(request.ChatId,
                                             int.Parse(request.Uid),
                                             token);
            response.MessageRemovedStatus = MessageRemovedStatus.Ok;
        }
        catch
        {
            response.MessageRemovedStatus = MessageRemovedStatus.Fail;
        }

        response.MessageUid = request.Uid;

        var eventArgs = new MessageRemovedBotEventArgs
        {
            MessageUid = request.Uid
        };

        MessageRemoved?.Invoke(this, eventArgs);
        Mediator?.Publish(eventArgs, token).Start();

        return response;
    }

    /// <summary>
    ///     Sends a message as a telegram bot
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="BotException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override async Task<SendMessageResponse> SendMessageAsync<TSendOptions>(SendMessageRequest request,
                                                                                   ISendOptionsBuilder<TSendOptions> optionsBuilder,
                                                                                   CancellationToken token)
    {
        if (!IsStarted)
        {
            Logger.LogInformation("Bot wasn't started!");

            return new SendMessageResponse(request.Uid, "Bot wasn't started!")
            {
                MessageSentStatus = MessageSentStatus.Nonstarted
            };
        }

        SendMessageResponse response = new(request.Uid, string.Empty);

        IReplyMarkup replyMarkup;

        if (optionsBuilder == default)
            replyMarkup = null;
        else if (optionsBuilder.Build() is IReplyMarkup)
            replyMarkup = optionsBuilder.Build() as IReplyMarkup;
        else
            replyMarkup = null;

        try
        {
            if (request?.Message == default) throw new BotException("request/message is null!");

            var text = new StringBuilder($"{request.Message.Subject} {request.Message.Body}");

            text = Escape(text);
            var retText = text.ToString();

            if (!string.IsNullOrWhiteSpace(retText))
                foreach (var chatId in request.Message.ChatIds)
                {
                    await _client.SendTextMessageAsync(chatId,
                                                       retText,
                                                       parseMode: ParseMode.MarkdownV2,
                                                       replyToMessageId: request.Message.ReplyToMessageUid != default ? int.Parse(request.Message.ReplyToMessageUid) : default,
                                                       replyMarkup: replyMarkup,
                                                       cancellationToken: token);

                    if (request.Message?.Poll != default)
                    {
                        var type = request.Message.Poll?.Type switch
                        {
                            Poll.PollType.Quiz    => PollType.Quiz,
                            Poll.PollType.Regular => PollType.Regular,
                            _                     => throw new ArgumentOutOfRangeException()
                        };

                        await _client.SendPollAsync(chatId,
                                                    request.Message.Poll?.Question,
                                                    request.Message.Poll?.Variants,
                                                    isAnonymous :request.Message.Poll?.IsAnonymous,
                                                    type: type,
                                                    replyToMessageId: request.Message.ReplyToMessageUid != default ? int.Parse(request.Message.ReplyToMessageUid) : default,
                                                    replyMarkup: replyMarkup,
                                                    cancellationToken: token);
                    }

                    if (request.Message?.Contact != default) await SendContact(request, token, replyMarkup);

                    if (request.Message?.Attachments == null) continue;

                    foreach (var genAttach in request.Message
                                                     .Attachments
                                                     .Where(a => a is BinaryAttachment))
                    {
                        var attachment = (BinaryAttachment) genAttach;

                        switch (attachment.MediaType)
                        {
                            case MediaType.Audio:
                                var audio = new InputFileStream(attachment.Data.ToStream(), attachment.Name);
                                await _client.SendAudioAsync(chatId,
                                                             audio,
                                                             caption: request.Message.Subject,
                                                             parseMode: ParseMode.MarkdownV2,
                                                             replyToMessageId: request.Message.ReplyToMessageUid != default ? int.Parse(request.Message.ReplyToMessageUid) : default,
                                                             replyMarkup: replyMarkup,
                                                             cancellationToken: token);

                                break;
                            case MediaType.Video:
                                var video = new InputFileStream(attachment.Data.ToStream(), attachment.Name);
                                await _client.SendVideoAsync(chatId,
                                                             video,
                                                             replyToMessageId: request.Message.ReplyToMessageUid != default ? int.Parse(request.Message.ReplyToMessageUid) : default,
                                                             replyMarkup: replyMarkup,
                                                             cancellationToken: token);

                                break;
                            case MediaType.Image:
                                var image = new InputFileStream(attachment.Data.ToStream(), attachment.Name);
                                await _client.SendPhotoAsync(chatId,
                                                             image,
                                                             replyToMessageId: request.Message.ReplyToMessageUid != default ? int.Parse(request.Message.ReplyToMessageUid) : default,
                                                             replyMarkup: replyMarkup,
                                                             cancellationToken: token);

                                break;
                            case MediaType.Voice:
                                var voice = new InputFileStream(attachment.Data.ToStream(), attachment.Name);
                                await _client.SendVoiceAsync(chatId,
                                                             voice,
                                                             caption: request.Message.Subject,
                                                             parseMode: ParseMode.MarkdownV2,
                                                             replyToMessageId: request.Message.ReplyToMessageUid != default ? int.Parse(request.Message.ReplyToMessageUid) : default,
                                                             replyMarkup: replyMarkup,
                                                             cancellationToken: token);

                                break;
                            case MediaType.Sticker:
                                InputFile sticker = string.IsNullOrWhiteSpace(attachment.Url) ?
                                        new InputFileStream(attachment.Data.ToStream(), attachment.Name) :
                                        new InputFileUrl(attachment.Url);

                                await _client.SendStickerAsync(chatId,
                                                               sticker,
                                                               replyToMessageId: request.Message.ReplyToMessageUid != default ? int.Parse(request.Message.ReplyToMessageUid) : default,
                                                               replyMarkup: replyMarkup,
                                                               cancellationToken: token);

                                break;
                            case MediaType.Contact:
                                await SendContact(request, token, replyMarkup);

                                break;
                            case MediaType.Document:
                                var doc = new InputFileStream(attachment.Data.ToStream(), attachment.Name);
                                await _client.SendDocumentAsync(chatId, 
                                    doc, 
                                    replyToMessageId: request.Message.ReplyToMessageUid != default ? int.Parse(request.Message.ReplyToMessageUid) : default,
                                    replyMarkup: replyMarkup,
                                    cancellationToken: token
                                    );
                                break;
                            case MediaType.Unknown:
                            case MediaType.Poll:
                            case MediaType.Text:
                            default:
                                // nothing to do
                                break;
                        }
                    }
                }

            response.MessageSentStatus = MessageSentStatus.Ok;

            var eventArgs = new MessageSentBotEventArgs
            {
                Message = request.Message
            };

            MessageSent?.Invoke(this, eventArgs);
            Mediator?.Publish(eventArgs, token).Start();
        }
        catch (Exception ex)
        {
            response.MessageSentStatus = MessageSentStatus.Fail;
            Logger.LogError(ex, ex.Message);
        }

        return response;
    }

    private async Task SendContact(SendMessageRequest request, CancellationToken token, IReplyMarkup replyMarkup)
    {
        foreach (var chatId in request.Message?.ChatIds)
            await _client.SendContactAsync(chatId,
                                           request.Message.Contact?.Phone,
                                           firstName: request.Message?.Contact?.Name,
                                           lastName:request.Message?.Contact?.Surname,
                                           replyToMessageId: request.Message.ReplyToMessageUid != default ? int.Parse(request.Message.ReplyToMessageUid) : default,
                                           replyMarkup: replyMarkup,
                                           cancellationToken: token);
    }

    /// <summary>
    ///     Autoescape for special symbols
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private static StringBuilder Escape(StringBuilder text) =>
            text.Replace("!", @"\!")
                .Replace("*", @"\*")
                .Replace("'", @"\'")
                .Replace(".", @"\.")
                .Replace("+", @"\+")
                .Replace("~", @"\~")
                .Replace("@", @"\@")
                .Replace("_", @"\_")
                .Replace("(", @"\(")
                .Replace(")", @"\)")
                .Replace("-", @"\-")
                .Replace("`", @"\`")
                .Replace("=", @"\=")
                .Replace(">", @"\>")
                .Replace("<", @"\<")
                .Replace("{", @"\{")
                .Replace("}", @"\}")
                .Replace("[", @"\[")
                .Replace("]", @"\]")
                .Replace("#", @"\#");

    /// <summary>
    ///     Starts a bot
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async Task<StartBotResponse> StartBotAsync(StartBotRequest request, CancellationToken token)
    {
        try
        {
            Logger.LogInformation($"{nameof(StartBotAsync)}...");
            var response = await base.StartBotAsync(request, token);

            if (IsStarted)
            {
                Logger.LogInformation($"{nameof(StartBotAsync)}: already started");

                return response;
            }

            if (response.Status != AdminCommandStatus.Ok || IsStarted) return response;

            // Rethrowing an event from BotUpdateHandler
            _handler.MessageReceived += (sender, e)
                    =>
            {
                MessageReceived?.Invoke(sender, e);
                Mediator?.Publish(e, token).Start();
            };

            _client.StartReceiving(_handler, cancellationToken: token);

            IsStarted = true;
            Logger.LogInformation($"{nameof(StartBotAsync)}: started");

            return response;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
        }

        return StartBotResponse.GetInstance(AdminCommandStatus.Fail, "error");
    }

    /// <summary>
    ///     Stops a bot
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async Task<StopBotResponse> StopBotAsync(StopBotRequest request, CancellationToken token)
    {
        try
        {
            Logger.LogInformation($"{nameof(StopBotAsync)}...");
            var response = await base.StopBotAsync(request, token);

            if (response.Status != AdminCommandStatus.Ok || !IsStarted) return response;

            await _client.CloseAsync(token);

            IsStarted = false;
            Logger.LogInformation($"{nameof(StopBotAsync)}: stopped");

            return response;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
        }

        return StopBotResponse.GetInstance(AdminCommandStatus.Fail, "error");
    }

    [Obsolete($"Use {nameof(SetBotContext)}")]
    public override async Task SetBotKey(string key, CancellationToken token)
    {
        var currentKey = _secureStorage.GetBotKey(BotDataUtils.GetBotId());

        if (currentKey?.Key != key)
        {
            var stopRequest = StopBotRequest.GetInstance();
            var startRequest = StartBotRequest.GetInstance();

            await StopBotAsync(stopRequest, token);

            _secureStorage.SetBotKey(currentKey?.Id, key);
            _client = new TelegramBotClient(key);

            await StartBotAsync(startRequest, token);
        }
    }

    public override async Task SetBotContext(BotContext context, CancellationToken token)
    {
        var currentContext = _secureStorage.GetBotContext(BotDataUtils.GetBotId());

        if (currentContext?.BotKey != context.BotKey)
        {
            var stopRequest = StopBotRequest.GetInstance();
            var startRequest = StartBotRequest.GetInstance();
            await StopBotAsync(stopRequest, token);

            _secureStorage.SetBotContext(context);
            _client = new TelegramBotClient(context.BotKey);

            await StartBotAsync(startRequest, token);
        }
    }
}