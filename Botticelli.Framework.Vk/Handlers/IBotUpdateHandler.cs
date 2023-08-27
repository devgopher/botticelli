using Botticelli.Framework.Events;
using Botticelli.Framework.Vk.API.Responses;

namespace Botticelli.Framework.Vk.Handlers;

public interface IBotUpdateHandler
{
    public delegate void MsgReceivedEventHandler(object sender, MessageReceivedBotEventArgs e);

    public Task HandleUpdateAsync(List<UpdateEvent> update, CancellationToken cancellationToken);

    public event MsgReceivedEventHandler MessageReceived;
}