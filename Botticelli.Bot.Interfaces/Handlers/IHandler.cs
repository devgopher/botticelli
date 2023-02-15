namespace Botticelli.Bot.Interfaces.Handlers
{
    public interface IHandler<in T>
    {
        public Task Handle(T input);
    }
}
