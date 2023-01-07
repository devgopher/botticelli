using Botticelli.Shared.API;

namespace Botticelli.Interfaces;

/// <summary>
/// Request processor interface
/// </summary>
public interface IRequestProcessor
{
    /// <summary>
    /// Process
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public Task ProcessAsync(BaseRequest response, CancellationToken token);
}