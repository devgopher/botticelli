using Botticelli.Framework.Events;
using MediatR;

namespace Botticelli.Client.Analytics.Handlers;

public class MessageSentHandler : IRequestHandler<MessageSentBotEventArgs>
{
    public async Task Handle(MessageSentBotEventArgs request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}