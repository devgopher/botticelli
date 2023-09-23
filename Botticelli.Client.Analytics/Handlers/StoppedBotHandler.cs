using Botticelli.Framework.Events;
using MediatR;

namespace Botticelli.Client.Analytics.Handlers;

public class StoppedBotHandler : IRequestHandler<StoppedBotEventArgs>
{
    public async Task Handle(StoppedBotEventArgs request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}