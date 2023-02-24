using Botticelli.Bot.Interfaces.Handlers;

namespace Botticelli.Framework.Handlers;

public abstract class HandlerBase<TRequest, TResponse> : IHandler<TRequest, TResponse>
{
    protected HandlerBase()
    {
    }

    public abstract Task<TResponse> Handle(TRequest input, CancellationToken token);
}