using Botticelli.Bot.Interfaces.Handlers;

namespace Botticelli.Framework.Handlers;

public abstract class HandlerBase<TRequest, TResponse> : IHandler<TRequest, TResponse>
{
    public abstract Task Handle(TRequest input, CancellationToken token);
}