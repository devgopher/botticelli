using Botticelli.Framework.Events;
using MediatR;

namespace Botticelli.Client.Analytics.Handlers;

public class StartedBotHandler : IRequestHandler<StartedBotEventArgs>
{
    public async Task Handle(StartedBotEventArgs request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}