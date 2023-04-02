using System.Text;
using Botticelli.Framework.Events;
using Botticelli.Framework.Exceptions;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Botticelli.Shared.Utils;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Botticelli.Framework.Telegram;

public class TelegramBot : BaseBot<TelegramBot>
{
    private readonly ITelegramBotClient _client;
    private readonly IServiceProvider _sp;
    public override event MsgSentEventHandler MessageSent;
    public override event MsgReceivedEventHandler MessageReceived;
    public override event MsgRemovedEventHandler MessageRemoved;
    public override event MessengerSpecificEventHandler MessengerSpecificEvent;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="client"></param>
    /// <param name="services"></param>
    public TelegramBot(ITelegramBotClient client, IServiceCollection services)
    {
        IsStarted = false;
        _client = client;
        _sp = services.BuildServiceProvider();
    }

    public override BotType Type => BotType.Telegram;

    public override async Task<RemoveMessageResponse> DeleteMessageAsync(RemoveMessageRequest request, CancellationToken token)
    {
        if (!IsStarted)
            return new RemoveMessageResponse(request.Uid, "Bot wasn't started!")
            {
                MessageRemovedStatus = MessageRemovedStatus.Nonstarted
            };

        RemoveMessageResponse response = new(request.Uid, string.Empty);

        try
        {
            if (string.IsNullOrWhiteSpace(request.Uid)) throw new BotException("request/message is null!");

            await _client.DeleteMessageAsync(request.ChatId,
                                             int.Parse(request.Uid),
                                             token);
            response.MessageRemovedStatus = MessageRemovedStatus.Ok;
        }
        catch (Exception ex)
        {
            response.MessageRemovedStatus = MessageRemovedStatus.Fail;
        }

        response.MessageUid = request.Uid;

        MessageRemoved?.Invoke(this, new MessageRemovedBotEventArgs()
        {
            MessageUid = request.Uid
        });

        return response;
    }

    /// <summary>
    ///     Sends a message as a telegram bot
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="BotException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override async Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken token)
    {
        if (!IsStarted)
            return new SendMessageResponse(request.Uid, "Bot wasn't started!")
            {
                MessageSentStatus = MessageSentStatus.Nonstarted
            };

        SendMessageResponse response = new(request.Uid, string.Empty);

        try
        {
            if (request?.Message == default) throw new BotException("request/message is null!");

            var text = new StringBuilder($"{request.Message.Subject} {request.Message.Body}");

            text = Escape(text);
            var retText = text.ToString();

            if (!string.IsNullOrWhiteSpace(retText))
                await _client.SendTextMessageAsync(request.Message.ChatId,
                                                   retText,
                                                   ParseMode.MarkdownV2,
                                                   cancellationToken: token);

            if (request.Message.Attachments != null)
                foreach (var genAttach in request.Message.Attachments.Where(a => a is BinaryAttachment))
                {
                    var attachment = (BinaryAttachment) genAttach;

                    switch (attachment.MediaType)
                    {
                        case MediaType.Audio:
                            var audio = new InputOnlineFile(attachment.Data.ToStream(), attachment.Name);
                            await _client.SendAudioAsync(request.Message.ChatId,
                                                         audio,
                                                         request.Message.Subject,
                                                         ParseMode.MarkdownV2,
                                                         cancellationToken: token);

                            break;
                        case MediaType.Video:
                            var video = new InputOnlineFile(attachment.Data.ToStream(), attachment.Name);
                            await _client.SendVideoAsync(request.Message.ChatId, video, cancellationToken: token);

                            break;
                        case MediaType.Image:
                            var image = new InputOnlineFile(attachment.Data.ToStream(), attachment.Name);
                            await _client.SendPhotoAsync(request.Message.ChatId, image, cancellationToken: token);

                            break;
                        case MediaType.Text:
                            // nothing to do
                            break;
                        case MediaType.Voice:
                            var voice = new InputOnlineFile(attachment.Data.ToStream(), attachment.Name);
                            await _client.SendVoiceAsync(request.Message.ChatId,
                                                         voice,
                                                         request.Message.Subject,
                                                         ParseMode.MarkdownV2,
                                                         cancellationToken: token);

                            break;
                        case MediaType.Sticker:
                            var sticker = string.IsNullOrWhiteSpace(attachment.Url) ?
                                    new InputOnlineFile(attachment.Data.ToStream(), attachment.Name) :
                                    new InputOnlineFile(attachment.Url);

                            await _client.SendStickerAsync(request.Message.ChatId,
                                                           sticker,
                                                           cancellationToken: token);

                            break;

                        case MediaType.Unknown: break;
                        default:                throw new ArgumentOutOfRangeException();
                    }
                }

            response.MessageSentStatus = MessageSentStatus.Ok;

            MessageSent?.Invoke(this, new MessageSentBotEventArgs()
            {
                Message = request.Message
            });
        }
        catch (Exception ex)
        {
            response.MessageSentStatus = MessageSentStatus.Fail;
        }

        return response;
    }

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
                .Replace("]", @"\]");

    /// <summary>
    ///     Starts a bot
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async Task<StartBotResponse> StartBotAsync(StartBotRequest request, CancellationToken token)
    {
        var response = await base.StartBotAsync(request, token);

        if (IsStarted) return response;

        if (response.Status != AdminCommandStatus.Ok || IsStarted) return response;

        var updateHandler = _sp.GetRequiredService<IBotUpdateHandler>();

        // Rethrowing an event from BotUpdateHandler
        updateHandler.MessageReceived += (sender, e) 
                => MessageReceived?.Invoke(sender, e);

        _client.StartReceiving(updateHandler, cancellationToken: token);
        
        IsStarted = true;

        return response;
    }

    /// <summary>
    ///     Stops a bot
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async Task<StopBotResponse> StopBotAsync(StopBotRequest request, CancellationToken token)
    {
        var response = await base.StopBotAsync(request, token);

        if (response.Status != AdminCommandStatus.Ok || !IsStarted) return response;

        await _client.CloseAsync(token);

        IsStarted = false;

        return response;
    }
}