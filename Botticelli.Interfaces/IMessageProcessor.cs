using Botticelli.Shared.ValueObjects;

namespace Botticelli.Interfaces;

/// <summary>
///     Request processor interface
/// </summary>
public interface IMessageProcessor
{
    /// <summary>
    ///     Process
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public Task ProcessAsync(Message message, CancellationToken token);
}