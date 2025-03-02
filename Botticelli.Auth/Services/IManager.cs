namespace Botticelli.Auth.Services;

/// <summary>
///     User/Roles management
/// </summary>
public interface IManager<TInfo>
{
    /// <summary>
    ///     Gets user list
    /// </summary>
    /// <param name="from"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public Task<IEnumerable<TInfo>> Get(int from = 0, int pageSize = 20);

    /// <summary>
    ///     Adds a new user
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public Task Add(TInfo info);

    /// <summary>
    ///     Updates a user
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public Task Update(TInfo info);
}