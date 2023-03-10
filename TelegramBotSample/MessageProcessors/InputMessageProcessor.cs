using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using Botticelli.Talks;

namespace TelegramBotSample.MessageProcessors;

public class InputMessageProcessor : IClientMessageProcessor
{
    private IBot _bot;
    private IServiceProvider _sp;

    public async Task ProcessAsync(Message message, CancellationToken token)
    {
        try
        {
            var speaker = _sp.GetRequiredService<ISpeaker>();
            var result = await speaker.Speak(message.Body, token);
            var resultMessage = new SendMessageRequest(Guid.NewGuid().ToString())
            {
                Message =
                {
                    ChatId = message.ChatId,
                    Attachments = new List<IAttachment>()
                }
            };

            resultMessage.Message.Attachments.Add(new BinaryAttachment(Guid.NewGuid().ToString(),
                                                                       "response",
                                                                       MediaType.Audio,
                                                                       string.Empty,
                                                                       result));
            await _bot.SendMessageAsync(resultMessage, token);
        }
        catch (Exception ex)
        {
            // todo:
        }
    }

    /// <summary>
    ///     Sets a bot (perhaps, we do it in another way)
    /// </summary>
    /// <param name="bot"></param>
    public void SetBot(IBot bot) => _bot = bot;

    public void SetServiceProvider(IServiceProvider sp)
        => _sp = sp;
}