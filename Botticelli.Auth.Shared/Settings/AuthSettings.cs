namespace Botticelli.Auth.Shared.Settings;

public class AuthSettings
{
    public string? ConnectionString { get; set; }
    public int TokenLifetimeMin { get; set; } = 60;
}