using Botticelli.Framework.Events;
using MediatR;

namespace Botticelli.Client.Analytics.Handlers;

public class MessageRemovedHandler : IRequestHandler<MessageRemovedBotEventArgs>
{
    public async Task Handle(MessageRemovedBotEventArgs request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}