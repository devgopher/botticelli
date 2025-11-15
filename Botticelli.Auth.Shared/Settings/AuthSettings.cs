namespace Botticelli.Auth.Shared.Settings;

public class AuthSettings
{
    public string? ConnectionString { get; init; }
    public int TokenLifetimeMin { get; init; } = 60;
}