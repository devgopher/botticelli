namespace Botticelli.Bot.Interfaces.Handlers;

public interface IHandler<in TRequest, TResponse>
{
    /// <summary>
    ///     Handle a request
    /// </summary>
    /// <param name="input">Request</param>
    /// <param name="token">Cancellation token</param>
    /// <returns></returns>
    public Task Handle(TRequest input, CancellationToken token);
}