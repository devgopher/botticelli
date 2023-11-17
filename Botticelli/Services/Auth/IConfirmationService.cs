using Microsoft.AspNetCore.Identity;

namespace Botticelli.Server.Services.Auth;

/// <summary>
///     Email confirmation service
/// </summary>
public interface IConfirmationService
{
    /// <summary>
    ///     Sends a confirm code
    /// </summary>
    /// <param name="user"></param>
    /// <param name="token"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    Task SendConfirmationCode(IdentityUser<string> user, CancellationToken token);

    /// <summary>
    ///     Updates a user
    /// </summary>
    /// <param name="srcToken"></param>
    /// <param name="user"></param>
    /// <param name="ct"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<bool> ConfirmCodeAsync(string srcToken, IdentityUser<string> user, CancellationToken ct);
}