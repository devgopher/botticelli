namespace Botticelli.Server.Data.Entities.Auth;

public class ApplicationUserPost
{
    public string? UserName { get; set; }
    public int StatusId { get; set; }
    public ICollection<string>? UserRoleIds { get; set; }
}