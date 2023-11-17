using System.ComponentModel.DataAnnotations;

namespace Botticelli.Shared.API.Admin.Requests;

public class LoginRequest
{
    public string Email { get; set; }

    [DataType(DataType.Password)] public string Password { get; set; }

    [Display(Name = "Remember Me")] public bool RememberMe { get; set; }
}