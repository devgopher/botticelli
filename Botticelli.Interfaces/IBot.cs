namespace Botticelli.Interfaces;

/// <summary>
/// Common interface for bots
/// </summary>
public interface IBot : IEventBasedBotAdminApi, IEventBasedBotClientApi
{

}

/// <summary>
/// Common interface for bots
/// </summary>
public interface IBot<T> : IBot
    where T: IBot<T>
{
}