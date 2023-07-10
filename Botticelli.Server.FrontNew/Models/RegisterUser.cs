using System.ComponentModel.DataAnnotations;

namespace Botticelli.Server.FrontNew.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email isn't valid!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }
    }
}
