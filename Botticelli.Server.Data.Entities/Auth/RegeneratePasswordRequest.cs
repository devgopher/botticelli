namespace Botticelli.Server.Data.Entities.Auth;

public class RegeneratePasswordRequest 
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
}