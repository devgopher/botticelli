using Botticelli.Auth.Dto.Credentials;

namespace Botticelli.Auth.Rules;

/// <summary>
///     Ruleset for bot user credentials
/// </summary>
/// <param name="rules"></param>
/// <typeparam name="TDto"></typeparam>
public sealed class CredentialsRules<TDto>(IEnumerable<Func<TDto, bool>> rules)
        where TDto : IBotAuthCredentials
{
    public Task<bool> Compare(TDto dto)
    {
        return Task.FromResult(rules.All(rule => rule(dto)));
    }
}