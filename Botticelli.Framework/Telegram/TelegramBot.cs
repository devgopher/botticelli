using Botticelli.Framework.Exceptions;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Botticelli.Shared.Utils;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Botticelli.Framework.Telegram
{
    public class TelegramBot : BaseBot<TelegramBot>
    {
        private readonly ITelegramBotClient _client;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="client"></param>
        public TelegramBot(ITelegramBotClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Sends a message as a telegram bot
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="BotException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override async Task<SendMessageResponse> SendAsync(SendMessageRequest request)
        {

            SendMessageResponse response = new(request.Uid, string.Empty);
            try
            {
                if (request?.Message == default)
                    throw new BotException("request/message is null!");

                var text = $"{request.Message.Subject} {request.Message.Body}";
                if (!string.IsNullOrWhiteSpace(text))
                {
                    await _client.SendTextMessageAsync(request.Message.ChatId, text, ParseMode.MarkdownV2);
                }

                if (request.Message.Attachments != null)
                {
                    foreach (var attachment in request.Message.Attachments)
                    {
                        switch (attachment.MediaType)
                        {
                            case MediaType.Audio:
                                var audio = new InputOnlineFile(attachment.Data.ToStream(), attachment.Name);
                                await _client.SendAudioAsync(request.Message.ChatId, audio, request.Message.Subject,
                                    ParseMode.MarkdownV2);
                                break;
                            case MediaType.Video:
                                var video = new InputOnlineFile(attachment.Data.ToStream(), attachment.Name);
                                await _client.SendVideoAsync(request.Message.ChatId, video);
                                break;
                            case MediaType.Text:
                                // nothing to do
                                break;
                            case MediaType.Voice:
                                var voice = new InputOnlineFile(attachment.Data.ToStream(), attachment.Name);
                                await _client.SendVoiceAsync(request.Message.ChatId, voice, request.Message.Subject,
                                    ParseMode.MarkdownV2);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                
                response.MessageSentStatus = MessageSentStatus.OK;
            }
            catch (Exception ex)
            {
                response.MessageSentStatus = MessageSentStatus.FAIL;
            }

            return response;
        }
    }
}
