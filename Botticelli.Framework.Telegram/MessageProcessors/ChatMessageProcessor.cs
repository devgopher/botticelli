using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Telegram.Bot;

namespace Botticelli.Framework.Telegram.MessageProcessors
{
    public class ChatMessageProcessor : IClientMessageProcessor
    {
        public async Task ProcessAsync(Message message, CancellationToken token)
        {
            string command;
            try
            {
                var chatId = Convert.ToInt64(message.ChatId);

                if (Regex.IsMatch(message.Body, simpleCommandPattern))
                {
                    command = Regex.Matches(message.Body, simpleCommandPattern)
                                   .FirstOrDefault()
                                   .Groups[1].Value;
                    await _cpFactory.Get(command)
                              .ProcessAsync(Convert.ToInt64(message.ChatId), token, null);
                }
                else if (Regex.IsMatch(message.Body, argsCommandPattern))
                {
                    var match = Regex.Matches(message.Body, argsCommandPattern)
                                     .FirstOrDefault();

                    command = match.Groups[1].Value;

                    var argsString = match.Groups[2].Value;

                    var args = Array.Empty<string>();
                   
                   if (!string.IsNullOrWhiteSpace(argsString))
                        args = argsString.Split(" ");

                    await _cpFactory.Get(command)
                              .ProcessAsync(chatId, token, args);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(BotUpdateHandler)}: {ex.Message}");
            }
        }

        private readonly ITelegramBotClient _botClient;
        private readonly CommandProcessorFactory _cpFactory;
        private readonly ILogger _logger;
        private const string simpleCommandPattern = @"\/([a-zA-Z0-9]*)$";
        private const string argsCommandPattern = @"\/([a-zA-Z0-9]*) (.*)";

        public void SetBot(IBot bot)
        {
            throw new NotImplementedException();
        }

        public void SetServiceProvider(IServiceProvider sp)
        {
            throw new NotImplementedException();
        }
    }
}
