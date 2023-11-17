namespace Botticelli.Server.Data.Entities.Auth;

public class UserAddRequest : DefaultUserAddRequest
{
    public string? Password { get; set; }
}