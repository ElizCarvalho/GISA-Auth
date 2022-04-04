using System.ComponentModel.DataAnnotations;

namespace GISA_Auth.Auth
{
    public class RegisterModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [StringLength(100, MinimumLength = 6)]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        [Required]
        public int Role { get; set; }

    }
}
