using Botticelli.Bot.Interfaces.Handlers;

namespace Botticelli.Bot.Interfaces.PubSub;

public interface ISubscriber<in THandler, TRequest, TResponse>
        where THandler : IHandler<TRequest, TResponse>

{
    public Task Subscribe(THandler handler, CancellationToken token);
}