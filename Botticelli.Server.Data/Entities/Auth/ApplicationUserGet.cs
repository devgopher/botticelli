namespace Botticelli.Server.Data.Entities.Auth;

public class ApplicationUserGet
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public ICollection<Guid?> UserRoleIds;
}