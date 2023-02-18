namespace Botticelli.Bot.Interfaces.Handlers;

public interface IHandler<in TRequest, TResponse>
{
    public Task<TResponse> Handle(TRequest input);
}