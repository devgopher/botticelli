using Botticelli.Auth.Dto;
using Botticelli.Auth.Dto.Credentials;

namespace Botticelli.Auth.Services;

/// <summary>
///     User additional identification
/// </summary>
public interface IIdentifier<in TAuthCredentials, TUserInfo>
        where TAuthCredentials : IBotAuthCredentials
{
    public Task<IdentifyResponse<TUserInfo>> Identify(TAuthCredentials dto);
}