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
    /// <returns></returns>
    public Task ProcessAsync(Message message, CancellationToken token);
}