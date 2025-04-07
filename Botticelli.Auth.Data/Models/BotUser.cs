using System.ComponentModel.DataAnnotations;

namespace Botticelli.Auth.Data.Models;

public class BotUser
{
    [MaxLength(256)]
    public required string UserId { get; set; }

    [MaxLength(256)]
    public required string UserName { get; set; }

    [MaxLength(256)]
    public string? NickName { get; set; }

    [EmailAddress]
    [MaxLength(254)]
    public string? Email { get; set; }

    [MaxLength(256)]
    public string? FirstName { get; set; }

    [MaxLength(256)]
    public string? LastName { get; set; }

    [MaxLength(16)]
    public string? Phone { get; set; }

    public required Guid RoleId { get; set; }
    public BotUserRole? Role { get; set; }
    public required bool IsActive { get; set; }
}