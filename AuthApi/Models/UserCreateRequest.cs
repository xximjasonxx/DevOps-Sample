
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    public class UserCreateRequest
    {
        [Required(ErrorMessage = "Email Address is a required field")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is a required field")]
        public string Password { get; set; }
    }
}