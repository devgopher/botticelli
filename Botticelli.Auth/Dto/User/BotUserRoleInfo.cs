using System.ComponentModel.DataAnnotations;

namespace Botticelli.Auth.Dto.User;

/// <summary>
///     User role info for responses
/// </summary>
public class BotUserRoleInfo
{
    [Key]
    public required Guid Id { get; set; }

    [MaxLength(16)]
    public required string RoleName { get; set; }

    [MaxLength(1024)]
    public required string Description { get; set; }

    public bool IsSuperUser { get; set; } = false;
}