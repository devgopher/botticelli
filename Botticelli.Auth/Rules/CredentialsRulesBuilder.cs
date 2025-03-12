using Botticelli.Auth.Dto.Credentials;

namespace Botticelli.Auth.Rules;

/// <summary>
///     Credentials checking rules builder
/// </summary>
/// <typeparam name="TDto">Credentials DTO</typeparam>
public class CredentialsRulesBuilder<TDto> where TDto : IBotAuthCredentials
{
    private readonly List<Func<TDto, bool>> _rules = [];

    public CredentialsRulesBuilder<TDto> AddRule(Func<TDto, bool> fieldsAction)
    {
        _rules.Add(fieldsAction);

        return this;
    }

    public CredentialsRules<TDto> Build()
    {
        return new CredentialsRules<TDto>(_rules);
    }
}