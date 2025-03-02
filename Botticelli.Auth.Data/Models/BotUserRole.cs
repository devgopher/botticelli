using System.ComponentModel.DataAnnotations;

namespace Botticelli.Auth.Data.Models;

public class BotUserRole
{
    [Key]
    public required Guid Id { get; set; }
    [MaxLength(16)]
    public required string RoleName { get; set; }

    [MaxLength(1024)]
    public required string Description { get; set; }
    public bool IsSuperUser { get; set; } = false;
}
