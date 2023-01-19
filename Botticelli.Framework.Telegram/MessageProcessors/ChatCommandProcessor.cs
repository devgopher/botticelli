using System.Text.RegularExpressions;
using Botticelli.Framework.Telegram.Handlers;
using Botticelli.Interfaces;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Botticelli.Framework.Telegram.MessageProcessors
{
    public class ChatCommandProcessor : IClientMessageProcessor
    {
        public async Task ProcessAsync(Message message, CancellationToken token)
        {
            string command;
                try
                {
                    if (Regex.IsMatch(text, simpleCommandPattern))
                    {
                        command = Regex.Matches(text, simpleCommandPattern)
                                       .FirstOrDefault()
                                       .Groups[1].Value;
                        _cpFactory.Get(command)
                                  .Process(chatId);
                    }
                    else if (Regex.IsMatch(text, argsCommandPattern))
                    {
                        command = Regex.Matches(text, argsCommandPattern)
                                       .FirstOrDefault()
                                       .Groups[1].Value;

                        var argsString = Regex.Matches(text, argsCommandPattern)
                                              .FirstOrDefault()
                                              .Groups[2].Value;


                        if (!string.IsNullOrWhiteSpace(argsString))
                            args = argsString.Split(" ").ToList();

                        _cpFactory.Get(command)
                                  .Process(chatId, args?.ToArray());
                    }
                    else
                    {
                        result = _messageTextManager.GetText("ErroneousCommand", SupportedLangs.EN);
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
    }
}
