
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    public class UserCreateRequest
    {
        [Required(ErrorMessage = "Email Address is a required field")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is a required field")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Username is a required field")]
        public string Username { get; set; }

        [Required(ErrorMessage = "First Name is a required field")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is a required field")]
        public string LastName { get; set; }
    }
}