namespace Botticelli.Framework.Handlers;

public abstract class HandlerBase<T> : IHandler<T>
{
    private readonly IHandler<T> _handler;

    public HandlerBase(IHandler<T> handler) => _handler = handler;

    public abstract Task Handle(T input);
}