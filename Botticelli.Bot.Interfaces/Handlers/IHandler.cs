namespace Botticelli.Bot.Interfaces.Handlers;

public interface IHandler<in TRequest, TResponse>
{
    public Task Handle(TRequest input, CancellationToken token);
}