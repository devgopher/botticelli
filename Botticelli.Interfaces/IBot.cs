namespace Botticelli.Interfaces;

/// <summary>
/// Common interface for bots
/// </summary>
public interface IBot<T> : IEventBasedBotAdminApi, IEventBasedBotClientApi
    where T: IBot<T>
{
}