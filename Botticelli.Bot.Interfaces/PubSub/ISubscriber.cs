using Botticelli.Bot.Interfaces.Handlers;

namespace Botticelli.Bot.Interfaces.PubSub
{
    public interface ISubscriber<in THandler, TMessage>
            where THandler : IHandler<TMessage>

    {
        public void Subscribe(THandler handler);
    }
}
