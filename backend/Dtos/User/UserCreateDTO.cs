using Jannara_Ecommerce.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.Dtos.User
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
