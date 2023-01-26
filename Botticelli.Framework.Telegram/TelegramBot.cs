using System.Text;
using Botticelli.Framework.Exceptions;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Botticelli.Shared.Utils;
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

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="client"></param>
    /// <param name="services"></param>
    public TelegramBot(ITelegramBotClient client, IServiceCollection services)
    {
        isStarted = false;
        _client = client;
        _sp = services.BuildServiceProvider();
    }

    public override BotType Type => BotType.Telegram;

    public override async Task<RemoveMessageResponse> DeleteMessageAsync(RemoveMessageRequest request, CancellationToken token)
    {
        if (!isStarted)
            return new RemoveMessageResponse(request.Uid, "Bot wasn't started!")
            {
                MessageRemovedStatus = MessageRemovedStatus.NONSTARTED
            };

        RemoveMessageResponse response = new(request.Uid, string.Empty);

        try
        {
            if (string.IsNullOrWhiteSpace(request.Uid)) throw new BotException("request/message is null!");

            await _client.DeleteMessageAsync(request.ChatId,
                                             int.Parse(request.Uid),
                                             token);
            response.MessageRemovedStatus = MessageRemovedStatus.OK;
        }
        catch (Exception ex)
        {
            response.MessageRemovedStatus = MessageRemovedStatus.FAIL;
        }

        response.MessageUid = request.Uid;

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
        if (!isStarted)
            return new SendMessageResponse(request.Uid, "Bot wasn't started!")
            {
                MessageSentStatus = MessageSentStatus.NONSTARTED
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
                foreach (var attachment in request.Message.Attachments)
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
                        default: throw new ArgumentOutOfRangeException();
                    }

            response.MessageSentStatus = MessageSentStatus.OK;
        }
        catch (Exception ex)
        {
            response.MessageSentStatus = MessageSentStatus.FAIL;
        }

        return response;
    }

    private static StringBuilder Escape(StringBuilder text) =>
            text.Replace("!", @"\!")
                .Replace("*", @"\*")
                .Replace("'", @"\'")
                .Replace(".", @"\.")
                .Replace("_", @"\_")
                .Replace("(", @"\(")
                .Replace(")", @"\)")
                .Replace("-", @"\-")
                .Replace("`", @"\`");

    /// <summary>
    ///     Starts a bot
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async Task<StartBotResponse> StartBotAsync(StartBotRequest request, CancellationToken token)
    {
        var response = await base.StartBotAsync(request, token);

        if (isStarted) return response;


        if (response.Status == AdminCommandStatus.OK && !isStarted)
        {
            _client.StartReceiving(_sp.GetRequiredService<IUpdateHandler>(), cancellationToken: token);
            isStarted = true;
        }

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

        if (response.Status != AdminCommandStatus.OK || !isStarted) return response;

        await _client.CloseAsync(token);

        isStarted = false;

        return response;
    }
}