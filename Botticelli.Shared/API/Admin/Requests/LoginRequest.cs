using System.ComponentModel.DataAnnotations;

namespace Botticelli.Shared.API.Admin.Requests;

public class LoginRequest
{
    public required string Email { get; set; }

    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
}