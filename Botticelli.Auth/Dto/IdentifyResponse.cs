namespace Botticelli.Auth.Dto;

/// <summary>
///     Indetify service response
/// </summary>
/// <param name="Success">Is success</param>
/// <param name="ErrorMessage">Message for errors</param>
/// <param name="User">User info if success</param>
public record IdentifyResponse<TUserInfo>(bool Success, string ErrorMessage, TUserInfo? User);