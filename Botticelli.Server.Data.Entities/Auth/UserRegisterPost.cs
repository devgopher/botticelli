namespace Botticelli.Server.Data.Entities.Auth;

public class UserRegisterPost
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}