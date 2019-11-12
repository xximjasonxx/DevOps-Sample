
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email Address is required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}