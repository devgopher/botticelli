using Botticelli.Framework.Events;
using Botticelli.Framework.Vk.Messages.API.Responses;

namespace Botticelli.Framework.Vk.Messages.Handlers;

public interface IBotUpdateHandler
{
    public delegate void MsgReceivedEventHandler(object sender, MessageReceivedBotEventArgs e);

    public Task HandleUpdateAsync(List<UpdateEvent> update, CancellationToken cancellationToken);

    public event MsgReceivedEventHandler MessageReceived;
}