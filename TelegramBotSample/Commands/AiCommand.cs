using Botticelli.Framework.Commands;

namespace TelegramBotSample.Commands;

public class AiCommand : ICommand
{
    public Guid Id { get; }
    public string Body { get; set; }
}