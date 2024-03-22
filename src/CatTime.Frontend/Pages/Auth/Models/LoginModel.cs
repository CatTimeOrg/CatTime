using System.ComponentModel.DataAnnotations;

namespace CatTime.Frontend.Pages.Auth.Models
{
    public class LoginModel
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
        public string? Password { get; set; }
    }
}
